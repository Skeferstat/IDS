using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;

namespace IdsLibrary.Serializing
{
    public static class Deserializer
    {
        /// <summary>
        /// Deserialize the basket send xml data.
        /// </summary>
        /// <param name="xmlData">Basket xml data.</param>
        /// <returns>Basket data.</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static BasketSend.typeWarenkorb? DeserializeBasketSend(string xmlData)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };

            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "IdsLibrary.Models.Basket.BasketSend.xsd";
            using Stream? xsdStream = assembly.GetManifestResourceStream(resourceName);
            if (xsdStream == null)
            {
                throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");
            }

            settings.Schemas.Add(null, XmlReader.Create(xsdStream));

            XmlSerializer serializer = new XmlSerializer(typeof(BasketSend.typeWarenkorb));
            using StringReader reader = new StringReader(xmlData);
            return serializer.Deserialize(reader) as BasketSend.typeWarenkorb;
        }

        public static BasketReceive.typeWarenkorb? DeserializeBasketReceive(string xmlData)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };

            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "IdsLibrary.Models.Basket.BasketReceive.xsd";
            using Stream? xsdStream = assembly.GetManifestResourceStream(resourceName);
            if (xsdStream == null)
            {
                throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");
            }

            settings.Schemas.Add(null, XmlReader.Create(xsdStream));

            XmlSerializer serializer = new XmlSerializer(typeof(BasketReceive.typeWarenkorb));
            using StringReader reader = new StringReader(xmlData);
            return serializer.Deserialize(reader) as BasketReceive.typeWarenkorb;
        }
    }
}
