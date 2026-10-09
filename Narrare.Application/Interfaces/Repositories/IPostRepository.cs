using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Repositories;

public interface IPostRepository : IRepository<Post>
{
    Task<List<Post>> GetByCategoryAsync(int categoryId);
}