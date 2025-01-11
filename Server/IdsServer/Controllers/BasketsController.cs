using Microsoft.AspNetCore.Mvc;
using System.Xml;
using AutoMapper;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Caching.Memory;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using IdsLibrary.Serializing;
using IdsServer.Models;
using JetBrains.Annotations;

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
            baskets = [];
        }

        return Ok(baskets);
    }

    [HttpPost]
    public Task<IActionResult> Receive()
    {
        if (Request.HasFormContentType == false || !Request.Form.ContainsKey("warenkorb"))
        {
            return Task.FromResult<IActionResult>(BadRequest("Invalid request format or missing 'warenkorb'."));
        }

        IFormCollection form = Request.Form;
        StringValues target = form["target"];
        StringValues basketXml = form["warenkorb"];

        try
        {
            var basket = Deserializer.DeserializeBasketReceive(basketXml);
            BasketDto basketDto = _mapper.Map<BasketDto>(basket);
            basketDto.RawXml = basketXml;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(1024);


            if (_cache.TryGetValue(ReceivedBasketsCacheKey, out List<BasketDto> baskets) == false)
            {
                baskets = [];
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

    [HttpGet("orderitems")]
    [CanBeNull]
    public List<OrderItemDto> GetOrderItems(string basketId, DataSourceLoadOptions loadOptions)
    {
        if (_cache.TryGetValue(ReceivedBasketsCacheKey, out List<BasketDto> baskets) == false)
        {
            return null;
        }

        BasketDto basket = baskets.FirstOrDefault(b => b.BasketId == basketId);
        return basket?.OrderDto.OrderItemDtos;
    }
}
