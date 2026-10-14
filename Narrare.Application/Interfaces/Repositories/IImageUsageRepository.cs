using System;
using System.Collections.Generic;
using System.Text;

using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Repositories;

public interface IImageUsageRepository : IRepository<ImageUsage>
{
    Task<List<ImageUsage>> GetByPostAsync(int postId);

    Task<List<ImageUsage>> GetByCommentAsync(int commentId);

    Task<List<ImageUsage>> GetGalleryImagesAsync();
}
