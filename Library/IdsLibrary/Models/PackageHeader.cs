using System;

namespace IdsLibrary.Models
{
    public class PackageHeader
    {
        public string? CustomerNumber { get; set; } 
        public string? UserName { get; set; }
        public string? Password{ get; set; }
        public ActionCode ActionCode { get; set; } = ActionCode.Unknown;
        public Uri? HookUri { get; set; } 
        public string? Version { get; set; } 
        public string? Target { get; set; }


        public Uri ShopUri { get; set; } = new Uri("http://localhost");
    }
}
