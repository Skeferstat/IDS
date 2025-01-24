namespace IdsServer.Database.Models;

public partial class FakeArticle
{
    public Guid Id { get; set; }
    public string ArticleNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal ListPrice { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}