namespace IdsServer.Library.Models;

public class OrderItemDto
{
    public string ArticleNumber { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal OfferPrice { get; set; }
    public decimal NetPrice { get; set; }
    public decimal PriceBasis { get; set; }
    public decimal Vat { get; set; }

    /// <summary>
    /// Percentage surcharge of the item. Discounts are transferred as negative surcharges.
    /// </summary>
    public decimal Supplement { get; set; }
    

    public string Currency { get; set; } = string.Empty;
}