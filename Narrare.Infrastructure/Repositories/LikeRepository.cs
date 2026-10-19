using Microsoft.EntityFrameworkCore;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Infrastructure.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly NarrareDbContext _context;

    public LikeRepository(NarrareDbContext context)
    {
        _context = context;
    }

    public async Task<List<Like>> GetByPostAsync(int postId)
    {
        return await _context.Likes
            .Where(like => like.PostId == postId)
            .ToListAsync();
    }

    public async Task<List<Like>> GetByCommentAsync(int commentId)
    {
        return await _context.Likes
            .Where(like => like.CommentId == commentId)
            .ToListAsync();
    }

    public async Task<Like?> GetByUserAndPostAsync(
        int userId, int postId)
    {
        return await _context.Likes
            .FirstOrDefaultAsync(like =>
                like.UserId == userId &&
                like.PostId == postId);
    }

    public async Task<Like?> GetByUserAndCommentAsync(
        int userId, int commentId)
    {
        return await _context.Likes
            .FirstOrDefaultAsync(like =>
                like.UserId == userId &&
                like.CommentId == commentId);
    }

    public async Task AddAsync(Like like)
    {
        await _context.Likes.AddAsync(like);
    }

    public void Delete(Like like)
    {
        _context.Likes.Remove(like);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
