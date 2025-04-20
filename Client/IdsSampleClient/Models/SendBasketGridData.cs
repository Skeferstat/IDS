

using BasketSend;

namespace IdsSampleClient.Models;
internal class BasketGridData
{
    public DateTime Date { get; set; }
    public string Time { get; set; }
    public string Version { get; set; }
    public List<typeOrderItem> OrderItems { get; set; }
}
