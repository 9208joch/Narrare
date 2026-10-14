using System.Collections.Generic;

namespace Narrare.Web.Services;

public class PostImagesApiService
{
    private readonly ApiService _apiService;

    public PostImagesApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<PostImageDto>?> GetByPostAsync(int postId)
    {
        return await _apiService.GetAsync<List<PostImageDto>>(
            $"api/Images/posts/{postId}/images");
    }
}

public class PostImageDto
{
    public int Id { get; set; }
    public int ImageId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public int SortOrder { get; set; }
}