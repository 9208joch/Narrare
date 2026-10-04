using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Services;

public interface ICommentService
{
    Task<List<Comment>> GetAllAsync();

    Task<Comment?> GetByIdAsync(int id);

    Task<List<Comment>> GetByPostAsync(int postId);

    Task<Comment> CreateAsync(Comment comment);

    Task<Comment?> UpdateAsync(int id, Comment comment);

    Task<bool> DeleteAsync(int id);
}