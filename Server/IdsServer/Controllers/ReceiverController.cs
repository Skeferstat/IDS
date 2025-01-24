using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BasketReceive;
using IdsLibrary.Serializing;
using IdsServer.Database;
using IdsServer.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace IdsServer.Controllers;

[Route("api/[controller]")]
public class ReceiverController : Controller
{
    private readonly ILogger _logger;
    private readonly AppDbContext _dbContext;
    /// <summary>
    /// Initializes a new instance of the <see cref="ReceiverController"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="dbContext">Database context.</param>
    public ReceiverController(ILogger<ReceiverController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
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
}