using System.IO;
using System.Net.Http;
using System.Xml.Serialization;
using BasketSend;

namespace IdsLibrary.Converter
{
    public static class IdsConverter
    {
        public static StringContent ConvertToStringContent(typeWarenkorb basket)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(typeWarenkorb));
            using StringWriter writer = new StringWriter();
            serializer.Serialize(writer, basket);
            string xmlString = writer.ToString();

            return new StringContent(xmlString, System.Text.Encoding.UTF8, "application/xml");
        }

        public static string ConvertToXml(typeWarenkorb basket)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(typeWarenkorb));
            using StringWriter writer = new StringWriter();
            serializer.Serialize(writer, basket);
            string xmlString = writer.ToString();

            return xmlString;
        }
    }
}
