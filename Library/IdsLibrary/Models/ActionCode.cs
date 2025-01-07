using Ardalis.SmartEnum;

namespace IdsLibrary.Models
{
    public sealed class ActionCode : SmartEnum<ActionCode, string>
    {
        public static readonly ActionCode Unknown = new ActionCode(nameof(Unknown), "");
        public static readonly ActionCode SendBasketToShop = new ActionCode(nameof(SendBasketToShop), "WKS");
        public static readonly ActionCode ReceiveBasketFromShop = new ActionCode(nameof(ReceiveBasketFromShop), "WKE");



        public ActionCode(string name, string value) : base(name, value)
        {
        }
    }
}
