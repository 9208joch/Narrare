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

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
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

        existingPost.Title = post.Title;
        existingPost.Content = post.Content;
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

        _postRepository.Delete(post);
        await _postRepository.SaveChangesAsync();

        return true;
    }
}