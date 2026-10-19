using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly BadWordFilterService _badWordFilterService;
    public CommentService(
    ICommentRepository commentRepository,
    BadWordFilterService badWordFilterService)
    {
        _commentRepository = commentRepository;
        _badWordFilterService = badWordFilterService;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        return await _commentRepository.GetAllAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _commentRepository.GetByIdAsync(id);
    }

    public async Task<List<Comment>> GetByPostAsync(int postId)
    {
        return await _commentRepository.GetByPostAsync(postId);
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        comment.Content = _badWordFilterService.Filter(comment.Content);

        comment.CreatedAt = DateTime.UtcNow;

        await _commentRepository.AddAsync(comment);
        await _commentRepository.SaveChangesAsync();

        return comment;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment comment)
    {
        var existingComment = await _commentRepository.GetByIdAsync(id);

        if (existingComment == null)
        {
            return null;
        }

        existingComment.Content = _badWordFilterService.Filter(comment.Content);
        existingComment.UpdatedAt = DateTime.UtcNow;

        await _commentRepository.SaveChangesAsync();

        return existingComment;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment == null)
        {
            return false;
        }

        _commentRepository.Delete(comment);
        await _commentRepository.SaveChangesAsync();
        
        return true;
    }
    public async Task<bool> DeleteAsync(int id, int deletedByUserId)
    {
        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment == null)
        {
            return false;
        }

        comment.IsDeleted = true;
        comment.DeletedByUserId = deletedByUserId;
        comment.DeletedAt = DateTime.UtcNow;
        comment.UpdatedAt = DateTime.UtcNow;

        await _commentRepository.SaveChangesAsync();

        return true;
    }
}