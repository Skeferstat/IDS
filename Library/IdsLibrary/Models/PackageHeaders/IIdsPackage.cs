using System.Net.Http.Headers;
using System;
using System.Net.Http;

namespace IdsLibrary.Models.PackageHeaders
{
    /// <summary>
    /// Constructed package to send data to the shop.
    /// </summary>
    public interface IIdsPackage
    {
        public Uri ShopUri { get; set; }
        public string Method { get; set; }
        public MultipartFormDataContent Content { get; set; }
        public HttpContentHeaders Headers { get; set; }
    }
}