using Microsoft.AspNetCore.Mvc;
using System.Xml;
using AutoMapper;
using Microsoft.Extensions.Primitives;
using IdsLibrary.Serializing;
using System.Xml.Serialization;
using BasketReceive;
using System.Text;
using IdsServer.Database;
using IdsServer.Database.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

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


    [HttpGet("id")]
    public ActionResult<FakeArticle> Get(Guid id)
    {
        FakeArticle article = _dbContext.Articles.FirstOrDefault(x => x.Id == id);
        if (article == null)
        {
            return NotFound();
        }
        return Ok(article);
    }

    [HttpGet("number")]
    public ActionResult<FakeArticle> GetByArticleNumber(string number)
    {
        FakeArticle article = _dbContext.Articles.FirstOrDefault(x => x.ArticleNumber.ToLower() == number.ToLower());
        if (article == null)
        {
            return NotFound();
        }
        return Ok(article);
    }
}
