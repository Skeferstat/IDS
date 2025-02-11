using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BasketReceive;
using IdsLibrary.Models;
using IdsLibrary.Serializing;
using IdsServer.Database;
using IdsServer.Database.Models;
using IdsServer.Pages;
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
        if (Request.HasFormContentType == false || Request.Form.ContainsKey("action") == false)
        {
            return Task.FromResult<IActionResult>(BadRequest("Invalid request format or missing action code."));
        }

        IFormCollection form = Request.Form;
        StringValues action = form["action"];

        ActionCode code = ActionCode.FromValue(action);

        switch (code)
        {
            case var _ when code == ActionCode.SendBasketToShop:
                return ReceiveBasket(form);

            case var _ when code == ActionCode.ArticleSearch:
                return SearchArticle(form);

            case var _ when code == ActionCode.ArticleDeeplink:
                return CreateDeeplink(form);

            default:
                throw new InvalidOperationException($"Unknown ActionCode: {code}");
        }
    }

    private Task<IActionResult> CreateDeeplink(IFormCollection form)
    {
        StringValues articleNumber = form["ghnummer"];
        FakeArticle article =
            _dbContext.Articles.FirstOrDefault(x => x.ArticleNumber.ToLower() == articleNumber.ToString().ToLower());
        if (article == null)
        {
            return Task.FromResult<IActionResult>(NotFound("Article not found"));
        }

        IActionResult result = RedirectToPage("/ArticleDetails", new { articleNumber = article.ArticleNumber });
        return Task.FromResult(result);
    }

    private Task<IActionResult> SearchArticle(IFormCollection form)
    {
        string searchTerm = form["searchTerm"];

        var articles =
            _dbContext.Articles.Where(x => x.ArticleNumber.ToLower().Contains(searchTerm.ToLower())
               || x.Name.ToLower().Contains(searchTerm.ToLower())
               || x.Description.ToLower().Contains(searchTerm.ToLower())
            );

        List<string> articleNumbers = articles.Select(x => x.ArticleNumber).ToList();
        if (articles.Any() == true)
        {
            IActionResult result = RedirectToPage("/articles", new ArticlesModel() { ArticleNumbers = articleNumbers });
            return Task.FromResult(result);
        };


        return Task.FromResult<IActionResult>(NotFound());
    }

    private Task<IActionResult> ReceiveBasket(IFormCollection form)
    {
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

            IActionResult result = RedirectToPage("/BasketDetails", new { id = dbBasket.Id });
            return Task.FromResult(result);
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