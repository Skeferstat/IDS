namespace IdsSampleClient;
public class AppSettings
{
    public ShopSettings Shop { get; set; } = new();
}


public class ShopSettings
{
    public string AuthUrl { get; set; } = string.Empty;
    public string AuthUsername { get; set; } = string.Empty;
    public string AuthPassword { get; set; } = string.Empty;
}