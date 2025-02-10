using Microsoft.AspNetCore.Mvc;
using IdsServer.Database;
using IdsServer.Database.Models;
using DevExtreme.AspNet.Mvc;
using IdsServer.Pages;
using Newtonsoft.Json;

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
    /// Get all articles with the desired article number or all articles.
    /// </summary>
    /// <param name="articleNumbers">Article numbers, separated by ','.</param>
    /// <returns>All articles with the desired article number or all articles.</returns>
    [HttpGet]
    public IActionResult Get(string articleNumbers = "")
    {
        _logger.LogInformation("Getting all articles.");
        List<string> numbers = JsonConvert.DeserializeObject<List<string>>(articleNumbers);
        List<FakeArticle> articles;

        if (numbers == null || numbers.Any() == false)
        {
            articles = _dbContext.Articles.ToList();
            return Ok(articles);
        }

        articles = _dbContext.Articles.Where(x => numbers.Contains(x.ArticleNumber)).ToList();
        return Ok(articles);
    }
}
