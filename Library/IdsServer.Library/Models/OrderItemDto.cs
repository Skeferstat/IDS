namespace IdsServer.Library.Models;

public class OrderItemDto
{
    public string ArticleNumber { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal OfferPrice { get; set; }
    public decimal NetPrice { get; set; }

    public string Currency { get; set; } = string.Empty;
}