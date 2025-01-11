namespace IdsServer.Models;

public class BasketDto
{
    public string BasketId => BasketInfoDto.Date + "_" + BasketInfoDto.Time;
    public BasketInfoDto BasketInfoDto { get; set; }
    public OrderDto OrderDto { get; set; }

    public string RawXml { get; set; } = string.Empty;
}