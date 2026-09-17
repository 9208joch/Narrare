using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;

namespace Narrare.Infrastructure.Repositories;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    private readonly NarrareDbContext _context;

    public CommentRepository(NarrareDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<List<Comment>> GetByPostAsync(int postId)
    {
        return await _context.Comments
            .Where(comment => comment.PostId == postId)
            .ToListAsync();
    }
}