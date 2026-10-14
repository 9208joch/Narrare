using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;

namespace Narrare.Infrastructure.Repositories;

public class UploadedImageRepository
    : Repository<UploadedImage>, IUploadedImageRepository
{
    private readonly NarrareDbContext _context;

    public UploadedImageRepository(NarrareDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<List<UploadedImage>> GetByUserAsync(int userId)
    {
        return await _context.UploadedImages
            .Where(image => image.UploadedByUserId == userId)
            .OrderByDescending(image => image.UploadedAt)
            .ToListAsync();
    }
    public async Task<UploadedImage?> GetByIdAsync(int id)
    {
        return await _context.UploadedImages
            .FirstOrDefaultAsync(image => image.Id == id);
    }
}
