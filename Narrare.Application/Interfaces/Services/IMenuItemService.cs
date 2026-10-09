using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;


namespace Narrare.Application.Interfaces.Services;

public interface IMenuItemService
{
    Task<List<MenuItem>> GetAllAsync();

    Task<MenuItem?> GetByIdAsync(int id);

    Task<MenuItem> CreateAsync(MenuItem menuItem);

    Task<MenuItem?> UpdateAsync(MenuItem menuItem);

    Task<bool> DeleteAsync(int id);
}