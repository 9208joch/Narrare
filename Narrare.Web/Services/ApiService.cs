using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace Narrare.Web.Services;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly CurrentUserService _currentUserService;

    public ApiService(
        IHttpClientFactory httpClientFactory,
        CurrentUserService currentUserService)
    {
        _httpClientFactory = httpClientFactory;
        _currentUserService = currentUserService;
    }

    private void AddUserIdHeader(HttpClient client)
    {
        client.DefaultRequestHeaders.Remove("X-User-Id");

        if (_currentUserService.CurrentUser != null)
        {
            client.DefaultRequestHeaders.Add(
                "X-User-Id",
                _currentUserService.CurrentUser.Id.ToString());
        }
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        var client = _httpClientFactory.CreateClient("NarrareApi");

        AddUserIdHeader(client);

        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return default;
        }
        
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(
     string url,
     TRequest data)
    {
        var client = _httpClientFactory.CreateClient("NarrareApi");

        AddUserIdHeader(client);

        var response = await client.PostAsJsonAsync(url, data);

        if (!response.IsSuccessStatusCode)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<bool> PostAsync<TRequest>(
        string url,
        TRequest data)
    {
        var client = _httpClientFactory.CreateClient("NarrareApi");

        AddUserIdHeader(client);

        var response = await client.PostAsJsonAsync(url, data);

        return response.IsSuccessStatusCode;
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(
        string url,
        TRequest data)
    {
        var client = _httpClientFactory.CreateClient("NarrareApi");

        AddUserIdHeader(client);

        var response = await client.PutAsJsonAsync(url, data);

        if (!response.IsSuccessStatusCode)
        {
            return default;
        }
        
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
    public async Task<bool> PutWithoutResponseAsync<TRequest>(
    string url,
    TRequest data)
    {
        var client = _httpClientFactory.CreateClient("NarrareApi");

        AddUserIdHeader(client);

        var response = await client.PutAsJsonAsync(url, data);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(string url)
    {
        var client = _httpClientFactory.CreateClient("NarrareApi");

        AddUserIdHeader(client);

        var response = await client.DeleteAsync(url);

        return response.IsSuccessStatusCode;
    }
    public async Task<TResponse?> UploadAsync<TResponse>(
    string url,
    IBrowserFile file)
    {
        var client = _httpClientFactory.CreateClient("NarrareApi");

        AddUserIdHeader(client);

        using var content = new MultipartFormDataContent();
        using var fileStream = file.OpenReadStream(20 * 1024 * 1024);
        using var streamContent = new StreamContent(fileStream);
        
        streamContent.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue(
                file.ContentType);

        content.Add(streamContent, "file", file.Name);

        var response = await client.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            throw new HttpRequestException(
                $"Bilduppladdningen misslyckades. " +
                $"Status: {(int)response.StatusCode} " +
                $"{response.StatusCode}. " +
                $"Svar: {error}");
        }
        

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
}