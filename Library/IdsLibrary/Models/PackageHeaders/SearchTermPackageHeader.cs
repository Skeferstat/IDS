using System;

namespace IdsLibrary.Models.PackageHeaders
{
    public class SearchTermPackageHeader : IPackageHeader
    {
        public string? CustomerNumber { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Uri HookUri { get; set; } = new Uri("http://localhost");
        public string? Version { get; set; } = "2.5";
        public string? Target { get; set; } = "TOP";

        public Uri ShopUri { get; set; } = new Uri("http://localhost");
    }
}
