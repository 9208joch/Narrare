using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly BadWordFilterService _badWordFilterService;
    public PostService(
    IPostRepository postRepository,
    BadWordFilterService badWordFilterService)
    {
        _postRepository = postRepository;
        _badWordFilterService = badWordFilterService;
    }
    
    public async Task<List<Post>> GetAllAsync()
    {
        return await _postRepository.GetAllAsync();
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        return await _postRepository.GetByIdAsync(id);
    }

    public async Task<List<Post>> GetByCategoryAsync(int categoryId)
    {
        return await _postRepository.GetByCategoryAsync(categoryId);
    }

    public async Task<Post> CreateAsync(Post post)
    {
        post.Title = _badWordFilterService.Filter(post.Title);
        post.Content = _badWordFilterService.Filter(post.Content);

        post.CreatedAt = DateTime.UtcNow;

        await _postRepository.AddAsync(post);
        await _postRepository.SaveChangesAsync();

        return post;
    }
    
    public async Task<Post?> UpdateAsync(int id, Post post)
    {
        var existingPost = await _postRepository.GetByIdAsync(id);

        if (existingPost == null)
        {
            return null;
        }

        existingPost.Title = _badWordFilterService.Filter(post.Title);
        existingPost.Content = _badWordFilterService.Filter(post.Content);
        existingPost.CategoryId = post.CategoryId;
        existingPost.UpdatedAt = DateTime.UtcNow;

        await _postRepository.SaveChangesAsync();

        return existingPost;
    }
    

    public async Task<bool> DeleteAsync(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);

        if (post == null)
        {
            return false;
        }

        // Markera inlägget som borttaget i stället för att radera det.
        post.IsDeleted = true;
        post.UpdatedAt = DateTime.UtcNow;

        await _postRepository.SaveChangesAsync();

        return true;
    }

}