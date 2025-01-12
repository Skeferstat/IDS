using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BasketSend;
using IdsLibrary.Models;
using IdsLibrary.Serializing;
using System.Xml.Serialization;
using FluentValidation;
using IdsLibrary.Models.PackageHeaders;
using IdsLibrary.Validators;

namespace IdsLibrary.Factories
{
    public class BasketSendPackageFactory : IIdsPackageFactory<BasketSendPackageHeader>
    {

        public async Task<IIdsPackage> CreatePackage(BasketSendPackageHeader packageHeader, string basketXml)
        {

            var validator = new BasketSendPackageHeaderValidator();
            var validationResult = await validator.ValidateAsync(packageHeader);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"PackageHeader is not valid: {errors}");
            }



            var content = new MultipartFormDataContent();

            if (string.IsNullOrEmpty(packageHeader!.CustomerNumber) == false)
                content.Add(new StringContent(packageHeader.CustomerNumber), PackageParameter.CustomNumber);
            if (string.IsNullOrEmpty(packageHeader.UserName) == false)
                content.Add(new StringContent(packageHeader.UserName), PackageParameter.UserName);
            if (string.IsNullOrEmpty(packageHeader.Password) == false)
                content.Add(new StringContent(packageHeader.Password), PackageParameter.Password);

            
            if (string.IsNullOrEmpty(packageHeader.HookUri?.ToString()) == false)
                content.Add(new StringContent(packageHeader.HookUri!.ToString()), PackageParameter.HookUri);

            if (packageHeader.Version != null)
                content.Add(new StringContent(packageHeader.Version), PackageParameter.Version);
            if (packageHeader.Target != null)
                content.Add(new StringContent(packageHeader.Target), PackageParameter.Target);

            var basket = Deserializer.DeserializeBasketSend(basketXml!);
            content.Add(ConvertToStringContent(basket), PackageParameter.Basket);

            content.Add(new StringContent(ActionCode.SendBasketToShop), PackageParameter.Action);


            var idsPackage = new IdsPackage()
            {
                ShopUri = packageHeader.ShopUri,
                Content = content,
                Headers = content.Headers
            };

            return idsPackage;
        }

        private static StringContent ConvertToStringContent(typeWarenkorb basket)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(typeWarenkorb));
            using StringWriter writer = new StringWriter();
            serializer.Serialize(writer, basket);
            string xmlString = writer.ToString();

            return new StringContent(xmlString, System.Text.Encoding.UTF8, "application/xml");
        }

    }
}
