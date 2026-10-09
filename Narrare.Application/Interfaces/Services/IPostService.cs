using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Services;

public interface IPostService
{
    Task<List<Post>> GetAllAsync();

    Task<Post?> GetByIdAsync(int id);

    Task<List<Post>> GetByCategoryAsync(int categoryId);

    Task<Post> CreateAsync(Post post);

    Task<Post?> UpdateAsync(int id, Post post);

    Task<bool> DeleteAsync(int id);
}