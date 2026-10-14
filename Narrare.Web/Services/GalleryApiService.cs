using System.Net.Http.Json;

namespace Narrare.Web.Services;

public class GalleryApiService
{
    private readonly HttpClient _httpClient;

    public GalleryApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<GalleryImageDto>> GetImagesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<GalleryImageDto>>(
            "api/Images/gallery") ?? new List<GalleryImageDto>();
    }
}

public class GalleryImageDto
{
    public int Id { get; set; }
    public int ImageId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}