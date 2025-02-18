using Microsoft.AspNetCore.Mvc;
using IdsServer.Database;
using IdsServer.Database.Models;
using DevExtreme.AspNet.Mvc;
using IdsServer.Pages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;
using AutoMapper;
using BasketSend;

namespace IdsServer.Controllers;

[Route("api/[controller]")]
public class ArticlesController : Controller
{
    private readonly ILogger _logger;
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArticlesController"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="dbContext">Database context.</param>
    /// <param name="httpClientFactory">Client factory for http.</param>
    /// <param name="mapper">Mapper.</param>
    /// >
    public ArticlesController(ILogger<BasketsController> logger, AppDbContext dbContext, IHttpClientFactory httpClientFactory, IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
        _httpClient = httpClientFactory.CreateClient();
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

    [HttpPost("send")]
    public async Task<IActionResult> SendToClient([FromBody] ArticlesSendRequest request)
    {
        string hookUrl = "https://wanted-supreme-mackerel.ngrok-free.app/api/articles";


        List<FakeArticle> fakeArticles = request.Articles;
        if (fakeArticles == null || fakeArticles.Count < 1)
        {
            _logger.LogError("Article not found.");
            return NotFound();
        }

        List<typeOrderItem> articles = _mapper.Map<List<typeOrderItem>>(fakeArticles);

        XmlSerializer serializer = new XmlSerializer(typeof(List<typeOrderItem>));
        await using StringWriter stringWriter = new StringWriter();

        serializer.Serialize(stringWriter, articles);

        StringContent content = new StringContent(stringWriter.ToString(), Encoding.UTF8, "application/xml");

        try
        {
            HttpResponseMessage response = await _httpClient.PostAsync(hookUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true });
            }

            return StatusCode((int)response.StatusCode, new { success = false });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while sending the basket to the client.");
            throw;
        }
    }


    public class ArticlesSendRequest
    {
        public List<FakeArticle> Articles { get; set; }
    }
}
