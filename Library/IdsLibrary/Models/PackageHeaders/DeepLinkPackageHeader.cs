using System;

namespace IdsLibrary.Models.PackageHeaders
{
    /// <summary>
    /// Package header to send a deeplink data to the shop.
    /// </summary>
    public class DeepLinkPackageHeader : IPackageHeader
    {
        public string? CustomerNumber { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Version { get; set; } = "2.5";
        public string? Target { get; set; } = "TOP";

        public Uri ShopUri { get; set; } = new Uri("http://localhost");
    }
}
