using Microsoft.AspNetCore.Mvc;
using System.Xml;
using AutoMapper;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Caching.Memory;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using IdsLibrary.Serializing;
using JetBrains.Annotations;
using System.Xml.Serialization;
using BasketReceive;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using IdsServer.Library.Models;
using IdsServer.Database;
using IdsServer.Database.Models;
using System.Threading.Tasks;
using System.Xml.Linq;
using static IdsServer.Controllers.BasketsController;
using Newtonsoft.Json;
using Throw;

namespace IdsServer.Controllers;

[Route("api/[controller]")]
public class BasketsController : Controller
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasketsController"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="mapper">Mapper.</param>
    /// <param name="httpClientFactory">Factory for creating <see cref="HttpClient"/> instances.</param>
    /// <param name="dbContext">Database context.</param>
    public BasketsController(ILogger<BasketsController> logger, IMapper mapper, IHttpClientFactory httpClientFactory, AppDbContext dbContext)
    {
        _logger = logger;
        _mapper = mapper;
        _httpClient = httpClientFactory.CreateClient();
        _dbContext = dbContext;
    }


    [HttpGet]
    public IActionResult Get()
    {
        var dbBaskets = _dbContext.Baskets.ToList();
        var baskets = new List<BasketDto>();
        foreach (Basket dbBasket in dbBaskets)
        {
            typeWarenkorb rawBasket = Deserializer.DeserializeBasketReceive(dbBasket.Data);
            BasketDto bskt = _mapper.Map<BasketDto>(rawBasket);
            bskt.BasketId = dbBasket.Id;
            bskt.RawXml = dbBasket.Data;
            bskt.HookUrl = new Uri(dbBasket.HookUrl);
            baskets.Add(bskt);
        }

        return Ok(baskets);
    }


    [HttpPut]
    public async Task<IActionResult> Update(Guid key, [FromForm] string values)
    {
        var dataFromDb = _dbContext.Baskets.FirstOrDefault(b => b.Id == key);
        if (dataFromDb == null)
        {
            return NotFound();
        }

        typeWarenkorb rawBasket = Deserializer.DeserializeBasketReceive(dataFromDb.Data);
        BasketDto bskt = _mapper.Map<BasketDto>(rawBasket);
        bskt.BasketId = dataFromDb.Id;
        bskt.HookUrl = new Uri(dataFromDb.HookUrl);
        JsonConvert.PopulateObject(values, bskt);


        var tw =  _mapper.Map<typeWarenkorb>(bskt);

        var serializer = new XmlSerializer(typeof(typeWarenkorb));
        await using var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, tw);

        StringContent content = new StringContent(stringWriter.ToString(), Encoding.UTF8, "application/xml");

      
        var dbBasket = _mapper.Map<Basket>(bskt);
        dbBasket.Data = content.ToString();
        dbBasket.LastUpdate = DateTime.UtcNow;
        _dbContext.Baskets.Add(dbBasket);



        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete(Guid key)    // DON'T CHANGE THE NAME OF THE PARAMETER
    {
        var basket = _dbContext.Baskets.FirstOrDefault(b => b.Id == key);
        if (basket == null)
        {
            return NotFound();
        }

        _dbContext.Baskets.Remove(basket);
        _dbContext.SaveChanges();

        return Ok();
    }


    [HttpPost]
    public Task<IActionResult> ReceiveFromClient()
    {
        if (Request.HasFormContentType == false || !Request.Form.ContainsKey("warenkorb"))
        {
            return Task.FromResult<IActionResult>(BadRequest("Invalid request format or missing 'warenkorb'."));
        }

        IFormCollection form = Request.Form;
        StringValues hookurl = form["hookurl"];
        StringValues basketXml = form["warenkorb"];

        try
        {
            var basket = Deserializer.DeserializeBasketReceive(basketXml);
            BasketDto basketDto = _mapper.Map<BasketDto>(basket);
            basketDto.RawXml = basketXml;
            basketDto.HookUrl = new Uri(hookurl);

            var dbBasket = _mapper.Map<Basket>(basketDto);
            dbBasket.LastUpdate = DateTime.UtcNow;
            _dbContext.Baskets.Add(dbBasket);

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while saving the basket to the database.");
                throw;
            }


            return Task.FromResult<IActionResult>(Ok(new
            {
                Success = true,
            }));
        }
        catch (InvalidOperationException exception)
        {
            _logger.LogError(exception.Message);
        }
        catch (XmlException exception)
        {
            _logger.LogError($"XML Error at line {exception.LineNumber}: {exception.Message}");
        }
        catch (NotSupportedException exception)
        {
            _logger.LogError($"Unsupported operation: {exception.Message}");
        }
        catch (Exception exception)
        {
            _logger.LogError($"An error occurred: {exception.Message}");
        }

        return Task.FromResult<IActionResult>(BadRequest(new
        {
            Success = false
        }));
    }



    [HttpPost("send")]
    public async Task<IActionResult> SendToClient()
    {
        string hookUrl = Request.Headers["Hook-Url"];

        using var reader = new StreamReader(Request.Body);
        string xmlData = await reader.ReadToEndAsync();
        typeWarenkorb basket = Deserializer.DeserializeBasketReceive(xmlData);

        if (basket == null)
        {
            _logger.LogError("Deserialization of basket data failed.");
            throw new InvalidOperationException("Deserialization of basket data failed.");
        }


        // Manipulate data.
        var now = DateTime.UtcNow;
        basket.WarenkorbInfo.Date = now;
        basket.WarenkorbInfo.Time = now;

        var serializer = new XmlSerializer(typeof(typeWarenkorb));
        await using var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, basket);

        var content = new StringContent(stringWriter.ToString(), Encoding.UTF8, "application/xml");

        try
        {
            HttpResponseMessage response = await _httpClient.PostAsync(hookUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true });
            }

            return StatusCode((int)response.StatusCode, new { success = false });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while sending the basket to the client.");
            throw;
        }
    }

    
   



}
