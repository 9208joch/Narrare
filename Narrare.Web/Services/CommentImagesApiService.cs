using System.Collections.Generic;

namespace Narrare.Web.Services;

public class CommentImagesApiService
{
    private readonly ApiService _apiService;

    public CommentImagesApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<CommentImageDto>?> GetByCommentAsync(int commentId)
    {
        return await _apiService.GetAsync<List<CommentImageDto>>(
            $"api/Images/comments/{commentId}/images");
    }
}

public class CommentImageDto
{
    public int Id { get; set; }
    public int ImageId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public int SortOrder { get; set; }
}