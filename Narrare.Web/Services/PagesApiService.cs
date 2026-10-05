using Narrare.Domain.Entities;

namespace Narrare.Web.Services;

public class PagesApiService
{
    private readonly ApiService _apiService;

    public PagesApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<Page>?> GetAllAsync()
    {
        return await _apiService.GetAsync<List<Page>>(
            "api/Pages");
    }
    
    public async Task<Page?> GetByIdAsync(int id)
    {
        return await _apiService.GetAsync<Page>(
            $"api/Pages/{id}");
    }

    public async Task<Page?> GetBySlugAsync(string slug)
    {
        return await _apiService.GetAsync<Page>(
            $"api/Pages/slug/{slug}");
    }

    public async Task<Page?> CreateAsync(Page page)
    {
        return await _apiService.PostAsync<Page, Page>(
            "api/Pages",
            page);
    }
    
    public async Task<Page?> UpdateAsync(int id, Page page)
    {
        return await _apiService.PutAsync<Page, Page>(
            $"api/Pages/{id}",
            page);
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiService.DeleteAsync(
            $"api/Pages/{id}");
    }
    public async Task<bool> IncrementVisitCountAsync(int id)
    {
        return await _apiService.PostAsync<object>(
            $"api/Pages/{id}/visit",
            new { });
    }
}