using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;

namespace Narrare.Infrastructure.Repositories;

public class PageRepository : IPageRepository
{
    private readonly NarrareDbContext _context;

    public PageRepository(NarrareDbContext context)
    {
        _context = context;
    }

    public async Task<List<Page>> GetAllAsync()
    {
        return await _context.Pages
            .OrderBy(page => page.Title)
            .ToListAsync();
    }

    public async Task<Page?> GetByIdAsync(int id)
    {
        return await _context.Pages
            .FirstOrDefaultAsync(page => page.Id == id);
    }

    public async Task<Page?> GetBySlugAsync(string slug)
    {
        return await _context.Pages
            .FirstOrDefaultAsync(page => page.Slug == slug);
    }

    public async Task<Page> CreateAsync(Page page)
    {
        _context.Pages.Add(page);

        await _context.SaveChangesAsync();

        return page;
    }

    public async Task<Page?> UpdateAsync(Page page)
    {
        var existingPage = await _context.Pages
            .FirstOrDefaultAsync(x => x.Id == page.Id);

        if (existingPage == null)
        {
            return null;
        }

        existingPage.Title = page.Title;
        existingPage.Slug = page.Slug;
        existingPage.Content = page.Content;

        await _context.SaveChangesAsync();

        return existingPage;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var page = await _context.Pages
            .FirstOrDefaultAsync(x => x.Id == id);

        if (page == null)
        {
            return false;
        }

        _context.Pages.Remove(page);

        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<bool> IncrementVisitCountAsync(int id)
    {
        var page = await _context.Pages
            .FirstOrDefaultAsync(x => x.Id == id);

        if (page == null)
        {
            return false;
        }

        page.VisitCount++;

        await _context.SaveChangesAsync();

        return true;
    }
}