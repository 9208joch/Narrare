using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;

namespace Narrare.Infrastructure.Repositories;

public class PageContentRepository : IPageContentRepository
{
    private readonly NarrareDbContext _context;

    public PageContentRepository(NarrareDbContext context)
    {
        _context = context;
    }

    public async Task<List<PageContent>> GetByPageIdAsync(int pageId)
    {
        return await _context.PageContents
            .Where(x => x.PageId == pageId)
            .OrderBy(x => x.Order)
            .ToListAsync();
    }

    public async Task<PageContent?> GetByIdAsync(int id)
    {
        return await _context.PageContents
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PageContent> CreateAsync(PageContent content)
    {
        _context.PageContents.Add(content);

        await _context.SaveChangesAsync();

        return content;
    }

    public async Task<PageContent?> UpdateAsync(PageContent content)
    {
        var existingContent = await _context.PageContents
            .FirstOrDefaultAsync(x => x.Id == content.Id);

        if (existingContent == null)
        {
            return null;
        }

        existingContent.PageId = content.PageId;
        existingContent.Type = content.Type;
        existingContent.Content = content.Content;
        existingContent.Order = content.Order;

        await _context.SaveChangesAsync();

        return existingContent;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var content = await _context.PageContents
            .FirstOrDefaultAsync(x => x.Id == id);

        if (content == null)
        {
            return false;
        }

        _context.PageContents.Remove(content);

        await _context.SaveChangesAsync();

        return true;
    }
}