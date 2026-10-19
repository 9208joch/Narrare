using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Application.Services;

public class LikeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;

    public LikeService(ILikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }

    public async Task<List<Like>> GetByPostAsync(int postId)
    {
        return await _likeRepository.GetByPostAsync(postId);
    }

    public async Task<List<Like>> GetByCommentAsync(int commentId)
    {
        return await _likeRepository.GetByCommentAsync(commentId);
    }

    public async Task<bool> TogglePostLikeAsync(int userId, int postId)
    {
        var existingLike =
            await _likeRepository.GetByUserAndPostAsync(userId, postId);

        if (existingLike != null)
        {
            _likeRepository.Delete(existingLike);
            await _likeRepository.SaveChangesAsync();

            return false;
        }

        var like = new Like
        {
            UserId = userId,
            PostId = postId,
            CreatedAt = DateTime.UtcNow
        };

        await _likeRepository.AddAsync(like);
        await _likeRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ToggleCommentLikeAsync(int userId, int commentId)
    {
        var existingLike =
            await _likeRepository.GetByUserAndCommentAsync(userId, commentId);

        if (existingLike != null)
        {
            _likeRepository.Delete(existingLike);
            await _likeRepository.SaveChangesAsync();

            return false;
        }

        var like = new Like
        {
            UserId = userId,
            CommentId = commentId,
            CreatedAt = DateTime.UtcNow
        };

        await _likeRepository.AddAsync(like);
        await _likeRepository.SaveChangesAsync();

        return true;
    }
}
