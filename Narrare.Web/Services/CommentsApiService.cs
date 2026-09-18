using Narrare.Application.DTOs;

namespace Narrare.Web.Services;

public class CommentsApiService
{
    private readonly ApiService _apiService;

    public CommentsApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<CommentDto>?> GetAllAsync()
    {
        return await _apiService.GetAsync<List<CommentDto>>("api/Comments");
    }

    public async Task<List<CommentDto>?> GetByPostAsync(int postId)
    {
        return await _apiService.GetAsync<List<CommentDto>>(
            $"api/Comments/post/{postId}"
        );
    }
}