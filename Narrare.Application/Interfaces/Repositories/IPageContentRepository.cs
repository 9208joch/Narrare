using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Repositories;

public interface IPageContentRepository
{
    Task<List<PageContent>> GetByPageIdAsync(int pageId);

    Task<PageContent?> GetByIdAsync(int id);

    Task<PageContent> CreateAsync(PageContent content);

    Task<PageContent?> UpdateAsync(PageContent content);

    Task<bool> DeleteAsync(int id);
}