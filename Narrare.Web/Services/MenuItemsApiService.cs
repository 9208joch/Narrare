using Narrare.Application.DTOs;
using Narrare.Domain.Entities;

namespace Narrare.Web.Services;

public class MenuItemsApiService
{
    private readonly ApiService _apiService;

    public MenuItemsApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<MenuItemDto>?> GetAllAsync()
    {
        return await _apiService.GetAsync<List<MenuItemDto>>(
            "api/MenuItems");
    }

    public async Task<MenuItemDto?> GetByIdAsync(int id)
    {
        return await _apiService.GetAsync<MenuItemDto>(
            $"api/MenuItems/{id}");
    }

    public async Task<MenuItemDto?> CreateAsync(MenuItem menuItem)
    {
        return await _apiService.PostAsync<MenuItem, MenuItemDto>(
            "api/MenuItems",
            menuItem);
    }

    public async Task<MenuItemDto?> UpdateAsync(
        int id,
        MenuItem menuItem)
    {
        return await _apiService.PutAsync<MenuItem, MenuItemDto>(
            $"api/MenuItems/{id}",
            menuItem);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiService.DeleteAsync(
            $"api/MenuItems/{id}");
    }
}