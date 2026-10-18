
using Narrare.Application.DTOs;

namespace Narrare.Web.Services;

public class ContentReportsApiService
{
    private readonly ApiService _apiService;

    public ContentReportsApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<bool> CreateAsync(
        CreateContentReportDto request)
    {
        return await _apiService.PostAsync(
            "api/ContentReports",
            request);
    }


    public async Task<List<ContentReportDto>> GetAllAsync()
    {
        return await _apiService.GetAsync<List<ContentReportDto>>(
            "api/ContentReports") ?? new List<ContentReportDto>();
    }

    public async Task<bool> MarkAsReviewedAsync(int reportId)
    {
        return await _apiService.PutWithoutResponseAsync(
            $"api/ContentReports/{reportId}/review",
            new { });
    }



}
