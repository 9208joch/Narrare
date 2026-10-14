using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;

namespace Narrare.Infrastructure.Repositories;

public class ImageUsageRepository
    : Repository<ImageUsage>, IImageUsageRepository
{
    private readonly NarrareDbContext _context;

    public ImageUsageRepository(NarrareDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<List<ImageUsage>> GetByPostAsync(int postId)
    {
        return await _context.ImageUsages
            .Include(usage => usage.UploadedImage)
            .Where(usage => usage.PostId == postId)
            .OrderBy(usage => usage.SortOrder)
            .ToListAsync();
    }

    public async Task<List<ImageUsage>> GetByCommentAsync(int commentId)
    {
        return await _context.ImageUsages
            .Include(usage => usage.UploadedImage)
            .Where(usage => usage.CommentId == commentId)
            .OrderBy(usage => usage.SortOrder)
            .ToListAsync();
    }

    public async Task<List<ImageUsage>> GetGalleryImagesAsync()
    {
        return await _context.ImageUsages
            .Include(usage => usage.UploadedImage)
            .Where(usage => usage.IsInGallery)
            .OrderBy(usage => usage.SortOrder)
            .ToListAsync();
    }
}
