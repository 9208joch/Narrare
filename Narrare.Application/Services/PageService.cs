using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Application.Services;

public class PageService : IPageService
{
    private readonly IPageRepository _pageRepository;

    public PageService(IPageRepository pageRepository)
    {
        _pageRepository = pageRepository;
    }

    public async Task<List<Page>> GetAllAsync()
    {
        return await _pageRepository.GetAllAsync();
    }

    public async Task<Page?> GetByIdAsync(int id)
    {
        return await _pageRepository.GetByIdAsync(id);
    }

    public async Task<Page?> GetBySlugAsync(string slug)
    {
        return await _pageRepository.GetBySlugAsync(slug);
    }

    public async Task<Page> CreateAsync(Page page)
    {
        return await _pageRepository.CreateAsync(page);
    }

    public async Task<Page?> UpdateAsync(Page page)
    {
        return await _pageRepository.UpdateAsync(page);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _pageRepository.DeleteAsync(id);
    }
}