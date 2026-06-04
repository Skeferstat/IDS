

using BasketSend;

namespace IdsSampleClient.Models;
internal class BasketGridData
{
    public DateTime Date { get; set; }
    public required string Time { get; set; }
    public required string Version { get; set; }
    public List<typeOrderItem>? OrderItems { get; set; }
}
