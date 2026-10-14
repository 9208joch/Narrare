using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Services;

public interface IImageService
{
    Task<List<ImageUsage>> GetByPostAsync(int postId);

    Task<List<ImageUsage>> GetByCommentAsync(int commentId);

    Task<List<ImageUsage>> GetGalleryImagesAsync();
    Task<UploadedImage> UploadAsync(
    Stream imageStream,
    string contentType,
    long fileSize,
    int uploadedByUserId);
    Task AddToGalleryAsync(int uploadedImageId);
    Task RemoveFromGalleryAsync(int uploadedImageId);
    Task<UploadedImage?> GetByIdAsync(int id);
    Task AddToPostAsync(int uploadedImageId, int postId);
    Task AddToCommentAsync(int uploadedImageId, int commentId);



}