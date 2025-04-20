using System.Text.Json;
using System.Text;

namespace DcmLibrary;

public class TrinityApiService
{
    private readonly HttpClient _httpClient;

    public TrinityApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var payload = new
        {
            username = username,
            password = password
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/login", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync(); // ggf. deserialize
        }

        var error = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Login failed: {error}");

        return null;
    }
}