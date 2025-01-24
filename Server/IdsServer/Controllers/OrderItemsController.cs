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

namespace IdsServer.Controllers;

[Route("api/[controller]")]
public class OrderItemsController : Controller
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderItemsController"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="mapper">Mapper.</param>
    /// <param name="dbContext">Database context.</param>
    public OrderItemsController(ILogger<OrderItemsController> logger, IMapper mapper, AppDbContext dbContext)
    {
        _logger = logger;
        _mapper = mapper;
        _dbContext = dbContext;
    }


    [HttpGet]
    [CanBeNull]
    public List<OrderItemDto> Get(Guid basketId, DataSourceLoadOptions loadOptions)
    {
        var dbBasket = _dbContext.Baskets.FirstOrDefault(b => b.Id == basketId);
        if (dbBasket == null)
        {
            return null;
        }

        var basket = Deserializer.DeserializeBasketReceive(dbBasket.Data);
        BasketDto basketDto = _mapper.Map<BasketDto>(basket);
        basketDto.BasketId = dbBasket.Id;
        basketDto.RawXml = dbBasket.Data;
        basketDto.HookUrl = new Uri(dbBasket.HookUrl);

        return basketDto?.OrderDto.OrderItemDtos;
    }

    [HttpPut]
    public async Task<IActionResult> Update(string key, [FromForm] string values)
    {
        
        return Ok();
    }

}
