using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Repositories;

public interface ICommentRepository : IRepository<Comment>
{
    Task<List<Comment>> GetByPostAsync(int postId);
}