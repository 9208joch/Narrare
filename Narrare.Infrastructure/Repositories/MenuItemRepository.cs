using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;

namespace Narrare.Infrastructure.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly NarrareDbContext _context;

    public MenuItemRepository(NarrareDbContext context)
    {
        _context = context;
    }

    public async Task<List<MenuItem>> GetAllAsync()
    {
        return await _context.MenuItems
            .OrderBy(menuItem => menuItem.SortOrder)
            .ToListAsync();
    }

    public async Task<MenuItem?> GetByIdAsync(int id)
    {
        return await _context.MenuItems
            .FirstOrDefaultAsync(menuItem => menuItem.Id == id);
    }

    public async Task<MenuItem> CreateAsync(MenuItem menuItem)
    {
        _context.MenuItems.Add(menuItem);

        await _context.SaveChangesAsync();

        return menuItem;
    }

    public async Task<MenuItem?> UpdateAsync(MenuItem menuItem)
    {
        var existingMenuItem = await _context.MenuItems
            .FirstOrDefaultAsync(x => x.Id == menuItem.Id);

        if (existingMenuItem == null)
        {
            return null;
        }

        existingMenuItem.Name = menuItem.Name;
        existingMenuItem.Url = menuItem.Url;
        existingMenuItem.SortOrder = menuItem.SortOrder;

        await _context.SaveChangesAsync();

        return existingMenuItem;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var menuItem = await _context.MenuItems
            .FirstOrDefaultAsync(x => x.Id == id);

        if (menuItem == null)
        {
            return false;
        }

        _context.MenuItems.Remove(menuItem);

        await _context.SaveChangesAsync();

        return true;
    }
}