using Microsoft.AspNetCore.Mvc;
using IdsServer.Database;
using IdsServer.Database.Models;

namespace IdsServer.Controllers;

[Route("api/[controller]")]
public class ArticlesController : Controller
{
    private readonly ILogger _logger;
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArticlesController"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="dbContext">Database context.</param>
    public ArticlesController(ILogger<BasketsController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }


    /// <summary>
    /// Get all articles.
    /// </summary>
    /// <returns>All articles.</returns>
    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Getting all articles.");
        List<FakeArticle> articles = _dbContext.Articles.ToList();
        return Ok(articles);
    }


    /// <summary>
    /// Get article by article number.
    /// </summary>
    /// <param name="no">Article number.</param>
    /// <returns>Article details.</returns>
    [HttpGet("{no}")]
    public ActionResult<FakeArticle> Get(string no)
    {
        _logger.LogInformation("Getting article by article number.");
        FakeArticle article = _dbContext.Articles.FirstOrDefault(x => x.ArticleNumber == no);
        if (article == null)
        {
            return NotFound();
        }

        return Ok(article);
    }
}
