using BasketReceive;

namespace IdsServer.Database.Models;

public partial class FakeArticle
{
    public Guid Id { get; set; }

    public string ArticleNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal NetPrice { get; set; }
    public decimal OfferPrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}