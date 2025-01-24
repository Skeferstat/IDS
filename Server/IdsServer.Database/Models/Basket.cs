using BasketReceive;
using IdsServer.Database.Converter;
using System.Xml.Serialization;

namespace IdsServer.Database.Models;

public partial class Basket
{
    public Guid Id { get; set; }

    public required typeWarenkorb RawBasket { get; set; }

    public string Xml => GetXml(RawBasket);

    public string HookUrl { get; set; } = string.Empty;

    public DateTimeOffset LastUpdate { get; set; }


    private static string GetXml(typeWarenkorb rawBasket)
    {
       var xml = XmlConverter.SerializeToXml(rawBasket);
       return xml;
    }

}