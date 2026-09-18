using System.Net.Http.Json;

namespace Narrare.Web.Services;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        var client = _httpClientFactory.CreateClient("NarrareApi");

        return await client.GetFromJsonAsync<T>(url);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(
        string url,
        TRequest data)
    {
        var client = _httpClientFactory.CreateClient("NarrareApi");

        var response = await client.PostAsJsonAsync(url, data);

        if (!response.IsSuccessStatusCode)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
}