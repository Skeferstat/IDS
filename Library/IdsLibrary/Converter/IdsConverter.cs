using System.IO;
using System.Net.Http;
using System.Xml.Serialization;
using BasketSend;

namespace IdsLibrary.Converter
{
    internal class IdsConverter
    {
        internal static StringContent ConvertToStringContent(typeWarenkorb basket)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(typeWarenkorb));
            using StringWriter writer = new StringWriter();
            serializer.Serialize(writer, basket);
            string xmlString = writer.ToString();

            return new StringContent(xmlString, System.Text.Encoding.UTF8, "application/xml");
        }

    }
}
