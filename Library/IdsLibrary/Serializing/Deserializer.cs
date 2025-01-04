using System.IO;
using System.Xml.Serialization;
using System.Xml;
using BasketReceive;

namespace IdsLibrary.Serializing
{
    public static class Deserializer
    {
        public static typeWarenkorb? DeserializeBasketReceive(string xmlData)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };
            settings.Schemas.Add(null, "../Models/Basket/BasketReceive.xsd");

            XmlSerializer serializer = new XmlSerializer(typeof(typeWarenkorb));
            using StringReader reader = new StringReader(xmlData);
            return serializer.Deserialize(reader) as typeWarenkorb;
        }
    }
}
