using System;

namespace IdsLibrary.Models.PackageHeaders
{
    /// <summary>
    /// Package header to send a search term to the shop.
    /// </summary>
    public class SearchTermPackageHeader : IPackageHeader
    {
        public string? CustomerNumber { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Version { get; set; } = "2.5";
        public string? Target { get; set; } = "TOP";

        public Uri ShopUri { get; set; } = new Uri("http://localhost");
    }
}
