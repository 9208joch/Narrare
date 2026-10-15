
using Narrare.Application.DTOs;
using Narrare.Domain.Entities;

namespace Narrare.Web.Services;

public class CategoriesApiService
{
    private readonly ApiService _apiService;

    public CategoriesApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<CategoryDto>?> GetAllAsync()
    {
        return await _apiService.GetAsync<List<CategoryDto>>(
            "api/Categories");
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        return await _apiService.GetAsync<CategoryDto>(
            $"api/Categories/{id}");
    }

    public async Task<CategoryDto?> CreateAsync(Category category)
    {
        return await _apiService.PostAsync<Category, CategoryDto>(
            "api/Categories", category);
    }

    public async Task<CategoryDto?> UpdateAsync(
        int id, Category category)
    {
        return await _apiService.PutAsync<Category, CategoryDto>(
            $"api/Categories/{id}", category);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiService.DeleteAsync(
            $"api/Categories/{id}");
    }
}
 