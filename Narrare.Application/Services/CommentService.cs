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

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
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

        existingComment.Content = comment.Content;
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
}