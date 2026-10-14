using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Repositories;

public interface IUploadedImageRepository : IRepository<UploadedImage>
{
    Task<List<UploadedImage>> GetByUserAsync(int userId);
    Task<UploadedImage?> GetByIdAsync(int id);
}
