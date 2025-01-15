using System;
using System.Globalization;

namespace IdsServer.Library.Models;

public class BasketDto
{
    public Guid BasketId { get; set; }
    public BasketInfoDto BasketInfoDto { get; set; } = new BasketInfoDto();
    public OrderDto OrderDto { get; set; } = new OrderDto();

    public string RawXml { get; set; } = string.Empty;
    public Uri HookUrl { get; set; } = new Uri("http://localhost");
}