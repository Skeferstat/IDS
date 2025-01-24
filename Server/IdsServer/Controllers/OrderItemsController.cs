using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevExtreme.AspNet.Mvc;
using BasketReceive;
using IdsServer.Database;
using IdsServer.Database.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.EntityFrameworkCore;

// ReSharper disable EntityFramework.NPlusOne.IncompleteDataQuery
// ReSharper disable EntityFramework.NPlusOne.IncompleteDataUsage

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
    public List<typeOrderItem> Get(Guid basketId, DataSourceLoadOptions loadOptions)
    {
        Basket dbBasket = _dbContext.Baskets.FirstOrDefault(b => b.Id == basketId);
        return dbBasket?.RawBasket?.Order.OrderItem.ToList();
    }

    [HttpPut]
    public async Task<IActionResult> Update(string key, string values)
    {
        var data = JsonSerializer.Deserialize<Dictionary<string, object>>(values);

        if (data != null && data.TryGetValue("basketId", out var basketId))
        {
            Basket basket = _dbContext.Baskets.FirstOrDefault(b => b.Id == Guid.Parse(basketId.ToString()));
            typeOrderItem article = basket!.RawBasket.Order.OrderItem.FirstOrDefault(o => o.ArtNo == key);
            if (article != null)
            {
                JsonConvert.PopulateObject(values, article);
                basket.LastUpdate = DateTimeOffset.Now;
                _dbContext.Entry(basket).State = EntityState.Modified;
                try
                {
                   await _dbContext.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "An error occurred while updating the basket to the database.");
                    throw;
                }

                return Ok();
            }
        }

        return BadRequest();    
    }

}
