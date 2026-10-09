using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Repositories;

public interface IMenuItemRepository
{
    Task<List<MenuItem>> GetAllAsync();

    Task<MenuItem?> GetByIdAsync(int id);

    Task<MenuItem> CreateAsync(MenuItem menuItem);

    Task<MenuItem?> UpdateAsync(MenuItem menuItem);

    Task<bool> DeleteAsync(int id);
}