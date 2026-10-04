using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Repositories;

public interface IPageRepository
{
    Task<List<Page>> GetAllAsync();

    Task<Page?> GetByIdAsync(int id);

    Task<Page?> GetBySlugAsync(string slug);

    Task<Page> CreateAsync(Page page);

    Task<Page?> UpdateAsync(Page page);

    Task<bool> DeleteAsync(int id);
}