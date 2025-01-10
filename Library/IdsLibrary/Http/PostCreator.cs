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
        public PackageHeader PackageHeader { get; }

        public PostCreator(PackageHeader packageHeader)
        {
            PackageHeader = packageHeader == null ? throw new ArgumentNullException(nameof(packageHeader)) : packageHeader;

        }

        public async Task<(Uri ShopUri, MemoryStream ContentStream, HttpContentHeaders Headers)> GetAsync(typeWarenkorb basket)
        {
            var validator = new PackageHeaderValidator();
            var validationResult = await validator.ValidateAsync(PackageHeader);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"PackageHeader is not valid: {errors}");
            }

            var content = new MultipartFormDataContent();

            if (string.IsNullOrEmpty(PackageHeader.CustomerNumber) == false)
                content.Add(new StringContent(PackageHeader.CustomerNumber), PackageParameter.CustomNumber);
            if (string.IsNullOrEmpty(PackageHeader.UserName) == false)
                content.Add(new StringContent(PackageHeader.UserName), PackageParameter.UserName);
            if (string.IsNullOrEmpty(PackageHeader.Password) == false)
                content.Add(new StringContent(PackageHeader.Password), PackageParameter.Password);

            content.Add(new StringContent(PackageHeader.ActionCode.Value), PackageParameter.Action);

            if (string.IsNullOrEmpty(PackageHeader.HookUri?.ToString()) == false)
                content.Add(new StringContent(PackageHeader.HookUri!.ToString()), PackageParameter.HookUri);

            if (PackageHeader.Version != null)
                content.Add(new StringContent(PackageHeader.Version), PackageParameter.Version);
            if (PackageHeader.Target != null)
                content.Add(new StringContent(PackageHeader.Target), PackageParameter.Target);


            content.Add(ConvertToStringContent(basket), PackageParameter.Basket);


            var memoryStream = new MemoryStream();
            content.CopyToAsync(memoryStream).Wait();
            memoryStream.Position = 0;

            return (PackageHeader.ShopUri, memoryStream, content.Headers);
        }


        public async Task<(Uri ShopUri, MemoryStream ContentStream, HttpContentHeaders Headers)> GetAsync(string searchTerm)
        {
            var validator = new PackageHeaderValidator();
            var validationResult = await validator.ValidateAsync(PackageHeader);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"PackageHeader is not valid: {errors}");
            }

            var content = new MultipartFormDataContent();

            if (string.IsNullOrEmpty(PackageHeader.CustomerNumber) == false)
                content.Add(new StringContent(PackageHeader.CustomerNumber), PackageParameter.CustomNumber);
            if (string.IsNullOrEmpty(PackageHeader.UserName) == false)
                content.Add(new StringContent(PackageHeader.UserName), PackageParameter.UserName);
            if (string.IsNullOrEmpty(PackageHeader.Password) == false)
                content.Add(new StringContent(PackageHeader.Password), PackageParameter.Password);

            content.Add(new StringContent(PackageHeader.ActionCode.Value), PackageParameter.Action);

            if (string.IsNullOrEmpty(PackageHeader.HookUri?.ToString()) == false)
                content.Add(new StringContent(PackageHeader.HookUri!.ToString()), PackageParameter.HookUri);

            if (PackageHeader.Version != null)
                content.Add(new StringContent(PackageHeader.Version), PackageParameter.Version);
            if (PackageHeader.Target != null)
                content.Add(new StringContent(PackageHeader.Target), PackageParameter.Target);


            content.Add(new StringContent(searchTerm), PackageParameter.SearchTerm);


            var memoryStream = new MemoryStream();
            content.CopyToAsync(memoryStream).Wait();
            memoryStream.Position = 0;

            return (PackageHeader.ShopUri, memoryStream, content.Headers);
        }


        public async Task<(Uri ShopUri, MemoryStream ContentStream, HttpContentHeaders Headers)> GetArticleDeepLinkAsync(string wholesaleArticleNumber)
        {
            var validator = new PackageHeaderValidator();
            var validationResult = await validator.ValidateAsync(PackageHeader);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"PackageHeader is not valid: {errors}");
            }

            var content = new MultipartFormDataContent();

            if (string.IsNullOrEmpty(PackageHeader.CustomerNumber) == false)
                content.Add(new StringContent(PackageHeader.CustomerNumber), PackageParameter.CustomNumber);
            if (string.IsNullOrEmpty(PackageHeader.UserName) == false)
                content.Add(new StringContent(PackageHeader.UserName), PackageParameter.UserName);
            if (string.IsNullOrEmpty(PackageHeader.Password) == false)
                content.Add(new StringContent(PackageHeader.Password), PackageParameter.Password);

            content.Add(new StringContent(PackageHeader.ActionCode.Value), PackageParameter.Action);

            if (string.IsNullOrEmpty(PackageHeader.HookUri?.ToString()) == false)
                content.Add(new StringContent(PackageHeader.HookUri!.ToString()), PackageParameter.HookUri);

            if (PackageHeader.Version != null)
                content.Add(new StringContent(PackageHeader.Version), PackageParameter.Version);
            if (PackageHeader.Target != null)
                content.Add(new StringContent(PackageHeader.Target), PackageParameter.Target);


            content.Add(new StringContent(wholesaleArticleNumber), PackageParameter.WholesaleArticleNumber);


            var memoryStream = new MemoryStream();
            content.CopyToAsync(memoryStream).Wait();
            memoryStream.Position = 0;

            return (PackageHeader.ShopUri, memoryStream, content.Headers);
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
