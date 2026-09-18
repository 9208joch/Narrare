using Narrare.Application.DTOs;
using Narrare.Domain.Entities;

namespace Narrare.Web.Services;

public class PostsApiService
{
    private readonly ApiService _apiService;

    public PostsApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<PostDto>?> GetAllAsync()
    {
        return await _apiService.GetAsync<List<PostDto>>("api/Posts");
    }
    public async Task<PostDto?> CreateAsync(Post post)
    {
        return await _apiService.PostAsync<Post, PostDto>(
            "api/Posts",
            post);
    }
}