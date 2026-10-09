using Narrare.Domain.Entities;

namespace Narrare.Web.Services;

public class SiteStyleApiService
{
    private readonly ApiService _apiService;

    public SiteStyleApiService(ApiService apiService)
    {
        _apiService = apiService;
    }
    
    public async Task<SiteStyle?> GetAsync()
    {
        return await _apiService.GetAsync<SiteStyle>(
            "api/SiteStyle");
    }

    public async Task<SiteStyle?> CreateAsync(
        SiteStyle siteStyle)
    {
        return await _apiService.PostAsync<SiteStyle, SiteStyle>(
            "api/SiteStyle",
            siteStyle);
    }

    public async Task<SiteStyle?> UpdateAsync(
        SiteStyle siteStyle)
    {
        return await _apiService.PutAsync<SiteStyle, SiteStyle>(
            "api/SiteStyle",
            siteStyle);
    }
}