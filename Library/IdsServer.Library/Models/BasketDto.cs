using System;

namespace IdsServer.Library.Models;

public class BasketDto
{
    public Guid BasketId { get; set; }
    public string RawXml { get; set; } = string.Empty;
    public Uri HookUrl { get; set; } = new("http://localhost");
}