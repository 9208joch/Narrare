using Narrare.Application.DTOs;

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
}