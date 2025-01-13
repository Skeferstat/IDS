using System.Net.Http.Headers;
using System;
using System.Net.Http;

namespace IdsLibrary.Models.PackageHeaders
{
    /// <summary>
    /// Constructed package to send data to the shop.
    /// </summary>
    public class IdsPackage : IIdsPackage
    {
        public Uri ShopUri { get; set; } = new Uri("http://localhost");
        public string Method { get; set; } = "POST";
        public MultipartFormDataContent Content { get; set; } = new MultipartFormDataContent();
        public HttpContentHeaders Headers { get; set; } = new MultipartFormDataContent().Headers;
    }
}