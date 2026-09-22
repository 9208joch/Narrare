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
    public async Task<T?> GetAsync<T>(
    string url,
    int userId)
    {
        var client = _httpClientFactory.CreateClient("NarrareApi");

        client.DefaultRequestHeaders.Remove("X-User-Id");
        client.DefaultRequestHeaders.Add("X-User-Id", userId.ToString());

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
    public async Task<TResponse?> PutAsync<TRequest, TResponse>(
    string url,
    TRequest data)
    {
        var client = _httpClientFactory.CreateClient("NarrareApi");

        var response = await client.PutAsJsonAsync(url, data);

        if (!response.IsSuccessStatusCode)
        {
            return default;
        }
        
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
    public async Task<bool> DeleteAsync(string url)
    {
        var client = _httpClientFactory.CreateClient("NarrareApi");

        var response = await client.DeleteAsync(url);

        return response.IsSuccessStatusCode;
    }
}