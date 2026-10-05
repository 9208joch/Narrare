using Narrare.Domain.Entities;

namespace Narrare.Web.Services;

public class PageContentApiService
{
    private readonly ApiService _apiService;

    public PageContentApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<PageContent>?> GetByPageIdAsync(int pageId)
    {
        return await _apiService.GetAsync<List<PageContent>>(
            $"api/PageContent/page/{pageId}");
    }

    public async Task<PageContent?> GetByIdAsync(int id)
    {
        return await _apiService.GetAsync<PageContent>(
            $"api/PageContent/{id}");
    }

    public async Task<PageContent?> CreateAsync(PageContent content)
    {
        return await _apiService.PostAsync<PageContent, PageContent>(
            "api/PageContent",
            content);
    }

    public async Task<PageContent?> UpdateAsync(
        int id,
        PageContent content)
    {
        return await _apiService.PutAsync<PageContent, PageContent>(
            $"api/PageContent/{id}",
            content);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiService.DeleteAsync(
            $"api/PageContent/{id}");
    }
}