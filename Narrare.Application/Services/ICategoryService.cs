using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(int id);
}