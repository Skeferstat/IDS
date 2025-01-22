using Microsoft.AspNetCore.Mvc;
using System.Xml;
using AutoMapper;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Caching.Memory;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using IdsLibrary.Serializing;
using JetBrains.Annotations;
using System.Xml.Serialization;
using BasketReceive;
using System.Text;
using System.Net.Http;
using IdsServer.Library.Models;
using IdsServer.Database;
using IdsServer.Database.Models;
using System.Threading.Tasks;
using System.Xml.Linq;
using static IdsServer.Controllers.BasketsController;

namespace IdsServer.Controllers;

[Route("api/[controller]")]
public class BasketsController : Controller
{

    private const string ReceivedBasketsCacheKey = "ReceivedBaskets";

    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasketsController"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="mapper">Mapper.</param>
    /// <param name="httpClientFactory">Factory for creating <see cref="HttpClient"/> instances.</param>
    /// <param name="dbContext">Database context.</param>
    public BasketsController(ILogger<BasketsController> logger, IMapper mapper, IHttpClientFactory httpClientFactory, AppDbContext dbContext)
    {
        _logger = logger;
        _mapper = mapper;
        _httpClient = httpClientFactory.CreateClient();
        _dbContext = dbContext;
    }


    [HttpGet]
    public IActionResult Get()
    {
        var dbBaskets = _dbContext.Baskets.ToList();
        var baskets = new List<BasketDto>();
        foreach (Basket dbBasket in dbBaskets)
        {
            typeWarenkorb rawBasket = Deserializer.DeserializeBasketReceive(dbBasket.Data);
            BasketDto bskt = _mapper.Map<BasketDto>(rawBasket);
            bskt.BasketId = dbBasket.Id;
            bskt.RawXml = dbBasket.Data;
            bskt.HookUrl = new Uri(dbBasket.HookUrl);
            baskets.Add(bskt);
        }

        return Ok(baskets);
    }

    [HttpPost]
    public Task<IActionResult> ReceiveFromClient()
    {
        if (Request.HasFormContentType == false || !Request.Form.ContainsKey("warenkorb"))
        {
            return Task.FromResult<IActionResult>(BadRequest("Invalid request format or missing 'warenkorb'."));
        }

        IFormCollection form = Request.Form;
        StringValues hookurl = form["hookurl"];
        StringValues basketXml = form["warenkorb"];

        try
        {
            var basket = Deserializer.DeserializeBasketReceive(basketXml);
            BasketDto basketDto = _mapper.Map<BasketDto>(basket);
            basketDto.RawXml = basketXml;
            basketDto.HookUrl = new Uri(hookurl);

            var dbBasket = _mapper.Map<Basket>(basketDto);
            dbBasket.LastUpdate = DateTime.UtcNow;
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


    [HttpPut]
    public async Task<IActionResult> UpdateBasket(int key, [FromForm] string values)
    {
        return Ok();
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveBasketData([FromBody] TreeBasket treeBasket)
    {
        var xml = ConvertToXml(treeBasket.Items);
        var basket = _dbContext.Baskets.FirstOrDefault(b => b.Id == treeBasket.Id);
        if (basket == null)
        {
            throw new InvalidOperationException("Basket not found.");
        }

        basket.Data = xml;
        basket.LastUpdate = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("orderitems")]
    [CanBeNull]
    public List<OrderItemDto> GetOrderItems(Guid basketId, DataSourceLoadOptions loadOptions)
    {
        var dbBasket = _dbContext.Baskets.FirstOrDefault(b => b.Id == basketId);
        if (dbBasket == null)
        {
            return null;
        }

        var basket = Deserializer.DeserializeBasketReceive(dbBasket.Data);
        BasketDto basketDto = _mapper.Map<BasketDto>(basket);
        basketDto.BasketId = dbBasket.Id;
        basketDto.RawXml = dbBasket.Data;
        basketDto.HookUrl = new Uri(dbBasket.HookUrl);

        return basketDto?.OrderDto.OrderItemDtos;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendToClient()
    {
        string hookUrl = Request.Headers["Hook-Url"];

        using var reader = new StreamReader(Request.Body);
        string xmlData = await reader.ReadToEndAsync();
        typeWarenkorb basket = Deserializer.DeserializeBasketReceive(xmlData);

        if (basket == null)
        {
            _logger.LogError("Deserialization of basket data failed.");
            throw new InvalidOperationException("Deserialization of basket data failed.");
        }


        // Manipulate data.
        var now = DateTime.UtcNow;
        basket.WarenkorbInfo.Date = now;
        basket.WarenkorbInfo.Time = now;

        var serializer = new XmlSerializer(typeof(typeWarenkorb));
        await using var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, basket);

        var content = new StringContent(stringWriter.ToString(), Encoding.UTF8, "application/xml");

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


    [HttpGet("raw")]
    public IActionResult GetRawBasket(Guid basketId)
    {

        var basket = _dbContext.Baskets.FirstOrDefault(b => b.Id == basketId);
        if (basket == null)
        {
            return NotFound();
        }

        typeWarenkorb bskt = Deserializer.DeserializeBasketReceive(basket.Data);
        return Ok(bskt);
    }

    [HttpDelete]
    public IActionResult Delete(Guid key)    // DON'T CHANGE THE NAME OF THE PARAMETER
    {
        var basket = _dbContext.Baskets.FirstOrDefault(b => b.Id == key);
        if (basket == null)
        {
            return NotFound();
        }

        _dbContext.Baskets.Remove(basket);
        _dbContext.SaveChanges();

        return Ok();
    }


    [HttpGet("tree")]
    public List<TreeNode> GetTree(string basketId)
    {
        var basket = _dbContext.Baskets.FirstOrDefault(b => b.Id == Guid.Parse(basketId));
        if (basket == null)
        {
            return [];
        }

        XDocument doc = XDocument.Parse(basket.Data);
        List<TreeNode> treeNodes = [];
        int idCounter = 1;

        ParseElement(doc.Root, 0, treeNodes, ref idCounter);

        return treeNodes;
    }


    private static void ParseElement(XElement element, int parentId, List<TreeNode> nodes, ref int idCounter)
    {
        var currentNode = new TreeNode
        {
            Id = idCounter++,
            ParentId = parentId,
            Name = element.Name.LocalName,
            Value = element.HasElements ? null : element.Value.Trim()
        };

        nodes.Add(currentNode);

        foreach (var child in element.Elements())
        {
            ParseElement(child, currentNode.Id, nodes, ref idCounter);
        }
    }

    public class TreeNode
    {
        public int Id { get; set; } 
        public int ParentId { get; set; } 
        public string Name { get; set; } 
        public string Value { get; set; }
    }



    public class TreeBasket
    {
        public Guid Id { get; set; }
        public List<TreeBasketItem> Items { get; set; }
    }


    public class TreeBasketItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int Id { get; set; }
        public int? ParentId { get; set; }
    }


    internal static string ConvertToXml(List<TreeBasketItem> items)
    {
        XNamespace ns = "http://www.itek.de/Shop-Anbindung/Warenkorb/";

        var root = items.FirstOrDefault(x => x.ParentId == 0);
        if (root == null) throw new InvalidOperationException("Root element not found.");

        XElement rootElement = BuildTree(root, items, ns);
        var xmlDeclaration = new XDeclaration("1.0", "UTF-8", "yes");
        var document = new XDocument(xmlDeclaration, rootElement);

        return document.ToString();
    }


    public static XElement BuildTree(TreeBasketItem parent, List<TreeBasketItem> allItems, XNamespace namespaceUri)
    {
        var children = allItems.Where(x => x.ParentId == parent.Id).ToList();
        var element = new XElement(namespaceUri + parent.Name);

        if (!string.IsNullOrEmpty(parent.Value))
        {
            element.Value = parent.Value;
        }

        foreach (var child in children)
        {
            element.Add(BuildTree(child, allItems, namespaceUri));
        }

        return element;
    }

}
