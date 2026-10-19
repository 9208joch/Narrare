
using Narrare.Application.DTOs;

namespace Narrare.Web.Services;

public class LikeApiService
{
    private readonly ApiService _apiService;

    public LikeApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<LikeToggleDto?> GetPostLikeAsync(int postId)
    {
        return await _apiService.GetAsync<LikeToggleDto>(
            $"api/Likes/post/{postId}");
    }

    public async Task<LikeToggleDto?> GetCommentLikeAsync(int commentId)
    {
        return await _apiService.GetAsync<LikeToggleDto>(
            $"api/Likes/comment/{commentId}");
    }

    public async Task<LikeToggleDto?> TogglePostLikeAsync(int postId)
    {
        return await _apiService.PostAsync<object, LikeToggleDto>(
            $"api/Likes/post/{postId}/toggle",
            new { });
    }

    public async Task<LikeToggleDto?> ToggleCommentLikeAsync(int commentId)
    {
        return await _apiService.PostAsync<object, LikeToggleDto>(
            $"api/Likes/comment/{commentId}/toggle",
            new { });
    }
}
