using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;

namespace Narrare.Infrastructure.Repositories;

public class PostRepository : Repository<Post>, IPostRepository
{
    private readonly NarrareDbContext _context;

    public PostRepository(NarrareDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<List<Post>> GetByCategoryAsync(int categoryId)
    {
        return await _context.Posts
            .Where(post => post.CategoryId == categoryId)
            .ToListAsync();
    }
}