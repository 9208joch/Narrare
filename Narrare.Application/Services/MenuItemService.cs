using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;
using Narrare.Application.Services;


namespace Narrare.Application.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;

    public MenuItemService(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<List<MenuItem>> GetAllAsync()
    {
        return await _menuItemRepository.GetAllAsync();
    }

    public async Task<MenuItem?> GetByIdAsync(int id)
    {
        return await _menuItemRepository.GetByIdAsync(id);
    }

    public async Task<MenuItem> CreateAsync(MenuItem menuItem)
    {
        return await _menuItemRepository.CreateAsync(menuItem);
    }

    public async Task<MenuItem?> UpdateAsync(MenuItem menuItem)
    {
        return await _menuItemRepository.UpdateAsync(menuItem);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _menuItemRepository.DeleteAsync(id);
    }
}