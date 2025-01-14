using Ardalis.SmartEnum;

namespace IdsLibrary.Models;

/// <summary>
/// Action code for the IDS package.
/// </summary>
public sealed class ActionCode : SmartEnum<ActionCode, string>
{
    public static readonly ActionCode Unknown = new ActionCode(nameof(Unknown), "");
    public static readonly ActionCode SendBasketToShop = new ActionCode(nameof(SendBasketToShop), "WKS");
    public static readonly ActionCode ArticleSearch = new ActionCode(nameof(ArticleSearch), "AS");
    public static readonly ActionCode ArticleDeeplink = new ActionCode(nameof(ArticleDeeplink), "ADL");

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionCode"/> class.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="value">Value.</param>
    public ActionCode(string name, string value) : base(name, value)
    {
    }
}

