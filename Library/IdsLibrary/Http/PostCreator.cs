using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using FluentValidation;
using IdsLibrary.Models;
using IdsLibrary.Validators;
using System.IO;
using BasketSend;
using System.Xml.Serialization;


namespace IdsLibrary.Http
{
    public class PostCreator
    {
        private readonly PackageHeader _packageHeader;

        public PostCreator(PackageHeader packageHeader)
        {
            _packageHeader = packageHeader == null ? throw new ArgumentNullException(nameof(packageHeader)) : packageHeader;

        }

        public async Task<(Uri ShopUri, MemoryStream ContentStream, HttpContentHeaders Headers)> GetAsync(typeWarenkorb basket)
        {
            var validator = new PackageHeaderValidator();
            var validationResult = await validator.ValidateAsync(_packageHeader);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"PackageHeader is not valid: {errors}");
            }

            var content = new MultipartFormDataContent();

            if (string.IsNullOrEmpty(_packageHeader.CustomerNumber) == false)
                content.Add(new StringContent(_packageHeader.CustomerNumber), PackageParameter.CustomNumber);
            if (string.IsNullOrEmpty(_packageHeader.UserName) == false)
                content.Add(new StringContent(_packageHeader.UserName), PackageParameter.UserName);
            if (string.IsNullOrEmpty(_packageHeader.Password) == false)
                content.Add(new StringContent(_packageHeader.Password), PackageParameter.Password);

            content.Add(new StringContent(_packageHeader.ActionCode.Value), PackageParameter.Action);

            if (string.IsNullOrEmpty(_packageHeader.HookUri?.ToString()) == false)
                content.Add(new StringContent(_packageHeader.HookUri!.ToString()), PackageParameter.HookUri);

            if (_packageHeader.Version != null)
                content.Add(new StringContent(_packageHeader.Version), PackageParameter.Version);
            if (_packageHeader.Target != null)
                content.Add(new StringContent(_packageHeader.Target), PackageParameter.Target);


            content.Add(ConvertToStringContent(basket), PackageParameter.Basket);


            var memoryStream = new MemoryStream();
            content.CopyToAsync(memoryStream).Wait();
            memoryStream.Position = 0;

            return (_packageHeader.ShopUri, memoryStream, content.Headers);
        }


        public static StringContent ConvertToStringContent(typeWarenkorb basket)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(typeWarenkorb));
            using StringWriter writer = new StringWriter();
            serializer.Serialize(writer, basket);
            string xmlString = writer.ToString();

            return new StringContent(xmlString, System.Text.Encoding.UTF8, "application/xml");
        }
    }
}
