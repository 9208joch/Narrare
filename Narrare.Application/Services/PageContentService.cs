using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Application.Services;

public class PageContentService : IPageContentService
{
    private readonly IPageContentRepository _pageContentRepository;

    public PageContentService(
        IPageContentRepository pageContentRepository)
    {
        _pageContentRepository = pageContentRepository;
    }

    public async Task<List<PageContent>> GetByPageIdAsync(int pageId)
    {
        return await _pageContentRepository.GetByPageIdAsync(pageId);
    }

    public async Task<PageContent?> GetByIdAsync(int id)
    {
        return await _pageContentRepository.GetByIdAsync(id);
    }

    public async Task<PageContent> CreateAsync(PageContent content)
    {
        return await _pageContentRepository.CreateAsync(content);
    }

    public async Task<PageContent?> UpdateAsync(PageContent content)
    {
        return await _pageContentRepository.UpdateAsync(content);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _pageContentRepository.DeleteAsync(id);
    }
}