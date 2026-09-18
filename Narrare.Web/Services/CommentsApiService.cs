using Narrare.Application.DTOs;
using Narrare.Domain.Entities;

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
    public async Task<CommentDto?> CreateAsync(Comment comment)
    {
        return await _apiService.PostAsync<Comment, CommentDto>(
            "api/Comments",
            comment);
    }
    public async Task<CommentDto?> UpdateAsync(int id, Comment comment)
    {
        return await _apiService.PutAsync<Comment, CommentDto>(
            $"api/Comments/{id}",
            comment);
    }
    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiService.DeleteAsync($"api/Comments/{id}");
    }

}