using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;
using System.Xml;
using AutoMapper;
using IdsServer.Models;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Caching.Memory;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;

namespace IdsServer.Controllers;

[Route("api/[controller]")]
public class BasketsController : Controller
{

    private const string ReceivedBasketsCacheKey = "ReceivedBaskets";

    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasketsController"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="mapper">Mapper.</param>
    /// <param name="cache">Memory cache.</param>
    public BasketsController(ILogger<BasketsController> logger, IMapper mapper, IMemoryCache cache)
    {
        _logger = logger;
        _mapper = mapper;
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }


    [HttpGet]
    public IActionResult Get()
    {
        if (_cache.TryGetValue(ReceivedBasketsCacheKey, out List<BasketDto> baskets) == false)
        {
            baskets = new List<BasketDto>();
        }

        return Ok(baskets);
    }

    [HttpPost]
    public Task<IActionResult> Receive()
    {

        if (!Request.HasFormContentType || !Request.Form.ContainsKey("warenkorb"))
        {
            return Task.FromResult<IActionResult>(BadRequest("Invalid request format or missing 'warenkorb'."));
        }


        IFormCollection form = Request.Form;
        StringValues target = form["target"];

        StringValues basketXml = form["warenkorb"];

        // Save the basket to a file
        System.IO.File.WriteAllText("basketrec.xml", basketXml);

        try
        {
            typeWarenkorb basket = DeserializeBasket(basketXml);
            BasketDto basketDto = _mapper.Map<BasketDto>(basket);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(1024);


            if (_cache.TryGetValue(ReceivedBasketsCacheKey, out List<BasketDto> baskets) == false)
            {
                baskets = new List<BasketDto>();
            }

            baskets.Add(basketDto);
            _cache.Set(ReceivedBasketsCacheKey, baskets, cacheEntryOptions);


            return Task.FromResult<IActionResult>(Ok(new
            {
                Success = true,
            }));
        }
        catch (InvalidOperationException exception)
        {
            Console.WriteLine($"Error: {exception.Message}");
        }
        catch (XmlException exception)
        {
            Console.WriteLine($"XML Error at line {exception.LineNumber}: {exception.Message}");
        }
        catch (NotSupportedException exception)
        {
            Console.WriteLine($"Unsupported operation: {exception.Message}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"An error occurred: {exception.Message}");
        }



        return Task.FromResult<IActionResult>(BadRequest(new
        {
            Success = false
        }));
    }



    [HttpGet("details")]
    public object GetDetails(string basketId, DataSourceLoadOptions loadOptions)
    {
        if (_cache.TryGetValue(ReceivedBasketsCacheKey, out List<BasketDto> baskets) == false)
        {
            baskets = new List<BasketDto>();
        }

        var f = DataSourceLoader.Load(
            from x in baskets
            where x.BasketId == basketId
            select x.OrderDto.OrderItemDtos,
            loadOptions
        );

        return f;
    }


    public static typeWarenkorb DeserializeBasket(string xmlData)
    {

        XmlReaderSettings settings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema
        };
        settings.Schemas.Add(null, "../IdsLibrary/Models/Basket/BasketReceive.xsd");

        XmlSerializer serializer = new XmlSerializer(typeof(typeWarenkorb));
        using StringReader reader = new StringReader(xmlData);
        return (typeWarenkorb)serializer.Deserialize(reader);
    }
}
