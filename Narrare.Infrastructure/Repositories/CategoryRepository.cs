using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;

namespace Narrare.Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(NarrareDbContext context)
        : base(context)
    {
    }
}