using Microsoft.AspNetCore.Mvc;
using System.Xml;
using AutoMapper;
using Microsoft.Extensions.Primitives;
using IdsLibrary.Serializing;
using System.Xml.Serialization;
using BasketReceive;
using System.Text;
using IdsServer.Database;
using IdsServer.Database.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace IdsServer.Controllers;

[Route("api/[controller]")]
public class BasketsController : Controller
{
    private readonly ILogger _logger;
    private readonly AppDbContext _dbContext;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasketsController"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="httpClientFactory">Factory for creating <see cref="HttpClient"/> instances.</param>
    /// <param name="dbContext">Database context.</param>
    public BasketsController(ILogger<BasketsController> logger, IHttpClientFactory httpClientFactory, AppDbContext dbContext)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
        _dbContext = dbContext;
    }


    [HttpGet]
    public ActionResult<List<Basket>> Get()
    {
        List<Basket> dbBaskets = _dbContext.Baskets.ToList();
        return Ok(dbBaskets);
    }

    [HttpGet("details")]
    public ActionResult<List<Basket>> GetBasket(Guid basketId)
    {
        List<Basket> dbBaskets = _dbContext.Baskets.Where(x => x.Id == basketId).ToList();
        return Ok(dbBaskets);
    }


    [HttpPut]
    public async Task<IActionResult> Update(Guid key, string values)
    {
        Basket dbBasket = _dbContext.Baskets.FirstOrDefault(b => b.Id == key);
        if (dbBasket == null)
        {
            return NotFound();
        }

        JsonConvert.PopulateObject(values, dbBasket);
        dbBasket.LastUpdate = DateTime.UtcNow;
        _dbContext.Entry(dbBasket).State = EntityState.Modified;
        try
        {
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while updating the basket to the database.");
            throw;
        }
    }

    [HttpDelete]
    public IActionResult Delete(Guid key)    // DON'T CHANGE THE NAME OF THE PARAMETER
    {
        Basket basket = _dbContext.Baskets.FirstOrDefault(b => b.Id == key);
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
        StringValues hookUrl = form["hookurl"];
        StringValues basketXml = form["warenkorb"];

        try
        {
            typeWarenkorb rawBasket = Deserializer.DeserializeBasketReceive(basketXml);
            Basket dbBasket = new Basket
            {
                Id = Guid.NewGuid(),
                RawBasket = rawBasket!,
                HookUrl = hookUrl,
                LastUpdate = DateTime.UtcNow
            };

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
    public async Task<IActionResult> SendToClient([FromBody]SendRequest sendRequest)
    {
        var basket = _dbContext.Baskets.FirstOrDefault(b => b.Id == sendRequest.BasketId);

        if (basket == null)
        {
            _logger.LogError("Basket not found.");
            return NotFound();
        }

        XmlSerializer serializer = new XmlSerializer(typeof(typeWarenkorb));
        await using StringWriter stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, basket.RawBasket);

        StringContent content = new StringContent(stringWriter.ToString(), Encoding.UTF8, "application/xml");

        try
        {
            HttpResponseMessage response = await _httpClient.PostAsync(basket.HookUrl, content);

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

public class SendRequest
{
    public Guid BasketId { get; set; }
}
