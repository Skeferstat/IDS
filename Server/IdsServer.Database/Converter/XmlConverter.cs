using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace IdsServer.Database.Converter;
public static class XmlConverter
{
    public static string SerializeToXml<T>(T value)
    {
        var serializer = new XmlSerializer(typeof(T));
        var encoding = Encoding.UTF8;

        var settings = new XmlWriterSettings
        {
            Encoding = encoding,
            Indent = true,
            OmitXmlDeclaration = true 
        };

        using var stringWriter = new StringWriter();
        using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
        {
            
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty); 
            serializer.Serialize(xmlWriter, value, namespaces);
        }

        var xml = stringWriter.ToString();
        var header = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>";
        return $"{header}\n{xml}";
    }

    public static T DeserializeFromXml<T>(string xml)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var stringReader = new StringReader(xml);
        return (T)serializer.Deserialize(stringReader)!;
    }
}
