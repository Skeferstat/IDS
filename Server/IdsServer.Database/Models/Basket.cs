namespace IdsServer.Database.Models;

public partial class Basket
{
    public Guid Id { get; set; }
    public string Data { get; set; } = string.Empty;

    public DateTimeOffset LastUpdate { get; set; }

}