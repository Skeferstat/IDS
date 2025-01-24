using System;
using System.Globalization;

namespace IdsServer.Library.Models;

public class BasketDto
{
    public Guid BasketId { get; set; }
    public BasketInfoDto BasketInfoDto { get; set; } = new();
    public OrderDto OrderDto { get; set; } = new();

    public string RawXml { get; set; } = string.Empty;
    public Uri HookUrl { get; set; } = new("http://localhost");
}