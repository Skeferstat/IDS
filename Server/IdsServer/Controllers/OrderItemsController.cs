using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevExtreme.AspNet.Mvc;
using BasketReceive;
using IdsServer.Database;
using IdsServer.Database.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.EntityFrameworkCore;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

// ReSharper disable EntityFramework.NPlusOne.IncompleteDataQuery
// ReSharper disable EntityFramework.NPlusOne.IncompleteDataUsage

namespace IdsServer.Controllers;

[Route("api/[controller]")]
public class OrderItemsController : Controller
{
    private readonly ILogger _logger;
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderItemsController"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="dbContext">Database context.</param>
    public OrderItemsController(ILogger<OrderItemsController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }


    [HttpGet]
    public LoadResult Get(Guid basketId, DataSourceLoadOptions loadOptions)
    {
        Basket dbBasket = _dbContext.Baskets.FirstOrDefault(b => b.Id == basketId);

        if (dbBasket != null && dbBasket?.RawBasket?.Order.OrderItem == null)
        {
            dbBasket.RawBasket.Order.OrderItem = [];
        }

        return DataSourceLoader.Load(dbBasket?.RawBasket.Order.OrderItem, loadOptions);
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


    [HttpPost]
    public async Task<ActionResult> Insert(string values, Guid basketId, DataSourceLoadOptions loadOptions)
    {
        var dbBasket = _dbContext.Baskets.FirstOrDefault(x => x.Id == basketId);
        if (dbBasket == null)
        {
            return NotFound();
        }

        typeOrderItem article = new();
        JsonConvert.PopulateObject(values, article);

        if (dbBasket?.RawBasket?.Order.OrderItem == null)
        {
            dbBasket.RawBasket.Order.OrderItem = [];
        }

        List<typeOrderItem> items = dbBasket.RawBasket.Order.OrderItem.ToList();
        items.Add(article);
        dbBasket.RawBasket.Order.OrderItem = items.ToArray();
        dbBasket.LastUpdate = DateTimeOffset.Now;
        _dbContext.Entry(dbBasket).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();


        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(string key, Guid basketId, DataSourceLoadOptions loadOptions)
    {
        var dbBasket = _dbContext.Baskets.FirstOrDefault(x => x.Id == basketId);
        if (dbBasket == null)
        {
            return NotFound();
        }

        typeOrderItem article = dbBasket!.RawBasket.Order.OrderItem.FirstOrDefault(o => o.ArtNo == key);
        if (article != null)
        {
            List<typeOrderItem> items = dbBasket.RawBasket.Order.OrderItem.ToList();
            items.Remove(article);
            dbBasket.RawBasket.Order.OrderItem = items.ToArray();
            dbBasket.LastUpdate = DateTimeOffset.Now;
            _dbContext.Entry(dbBasket).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        return Ok();
    }

}
