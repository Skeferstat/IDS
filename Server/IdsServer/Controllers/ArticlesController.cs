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


    [HttpGet]
    public IActionResult Get()
    {
        List<FakeArticle> articles = _dbContext.Articles.ToList();
        return Ok(articles);
    }


    [HttpGet("{id:guid}")]
    public ActionResult<FakeArticle> Get(Guid id)
    {
        FakeArticle article = _dbContext.Articles.FirstOrDefault(x => x.Id == id);
        if (article == null)
        {
            return NotFound();
        }

        return Ok(article);
    }
}
