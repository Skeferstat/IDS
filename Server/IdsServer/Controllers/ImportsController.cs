using Microsoft.AspNetCore.Mvc;
using IdsServer.Database;
using IdsServer.Database.Models;
using AutoMapper;
using Flurl;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using ImportLibrary;

namespace IdsServer.Controllers;

[Route("api/[controller]")]
public class ImportsController : Controller
{
    private readonly ILogger _logger;
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportsController"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="httpClientFactory">Client factory for http.</param>
    /// <param name="mapper">Mapper.</param>
    /// >
    public ImportsController(ILogger<BasketsController> logger, IHttpClientFactory httpClientFactory, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        _httpClient = httpClientFactory.CreateClient();
    }


    [HttpPost]
    public async Task<IActionResult> Import([FromForm] IFormFile xmlFile)
    {
        if (xmlFile == null || xmlFile.Length == 0)
        {
            return BadRequest("Keine Datei ausgewählt.");
        }

        using var stream = new MemoryStream();
        await xmlFile.CopyToAsync(stream);
        stream.Position = 0; 

        var serializer = new XmlSerializer(typeof(PositionModel));
        PositionModel xmlData = (PositionModel)serializer.Deserialize(stream);

        return Ok(new { FileName = xmlData.FileInfo.Name, OrderCount = xmlData.ArticleList.Count });
    }

    //[HttpPost]
    //public async Task<IActionResult> Import()
    //{
    //    string baseUrl = "https://trinityrestapi-test.datacrossmedia.de";
    //    string loginUrl = baseUrl.AppendPathSegment("login");

    //    var payload = new
    //    {
    //        username = "max@mustermann.de",
    //        password = "hunter2"
    //    };

    //    var json = JsonSerializer.Serialize(payload);
    //    var content = new StringContent(json, Encoding.UTF8, "application/json");

    //    try
    //    {
    //        HttpResponseMessage response = await _httpClient.PostAsync(loginUrl, content);

    //        if (response.IsSuccessStatusCode)
    //        {
    //            return Ok(new { success = true });
    //        }

    //        return StatusCode((int)response.StatusCode, new { success = false });
    //    }
    //    catch (Exception exception)
    //    {
    //        _logger.LogError(exception, "An error occurred while sending the basket to the client.");
    //        throw;
    //    }
    //}


    public class ArticlesSendRequest
    {
        public List<FakeArticle> Articles { get; set; }
    }
}
