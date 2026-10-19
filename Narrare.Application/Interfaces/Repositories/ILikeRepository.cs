using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Repositories;

public interface ILikeRepository
{
    Task<List<Like>> GetByPostAsync(int postId);

    Task<List<Like>> GetByCommentAsync(int commentId);

    Task<Like?> GetByUserAndPostAsync(int userId, int postId);

    Task<Like?> GetByUserAndCommentAsync(int userId, int commentId);

    Task AddAsync(Like like);

    void Delete(Like like);

    Task SaveChangesAsync();
}
