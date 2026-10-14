using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Application.Services;


public class ImageService : IImageService
{
    private readonly IImageUsageRepository _imageUsageRepository;
    private readonly IUploadedImageRepository _uploadedImageRepository;
    private readonly IImageStorageService _imageStorageService;
    private readonly IPostRepository _postRepository;
    private readonly ICommentService _commentService;
    
    public ImageService(
    IImageUsageRepository imageUsageRepository,
    IUploadedImageRepository uploadedImageRepository,
    IImageStorageService imageStorageService,
    IPostRepository postRepository,
    ICommentService commentService)
    {
        _imageUsageRepository = imageUsageRepository;
        _uploadedImageRepository = uploadedImageRepository;
        _imageStorageService = imageStorageService;
        _postRepository = postRepository;
        _commentService = commentService;
    }

    public async Task<List<ImageUsage>> GetByPostAsync(int postId)
    {
        return await _imageUsageRepository.GetByPostAsync(postId);
    }

    public async Task<List<ImageUsage>> GetByCommentAsync(int commentId)
    {
        return await _imageUsageRepository.GetByCommentAsync(commentId);
    }

    public async Task<List<ImageUsage>> GetGalleryImagesAsync()
    {
        return await _imageUsageRepository.GetGalleryImagesAsync();
    }


    public async Task<UploadedImage> UploadAsync(
        Stream imageStream,
        string contentType,
        long fileSize,
        int uploadedByUserId)
    {
        if (fileSize <= 0 || fileSize > 20 * 1024 * 1024)
        {
            throw new ArgumentException(
                "Bilden måste vara större än 0 byte och högst 20 MB.");
        }

        var storedImage = await _imageStorageService.SaveAsync(imageStream);

        var uploadedImage = new UploadedImage
        {
            FileName = storedImage.FileName,
            StoragePath = storedImage.FileName,
            ContentType = "image/jpeg",
            FileSize = storedImage.FileSize,
            UploadedAt = DateTime.UtcNow,
            UploadedByUserId = uploadedByUserId
        };

        try
        {
            await _uploadedImageRepository.AddAsync(uploadedImage);
            await _uploadedImageRepository.SaveChangesAsync();
        }
        catch
        {
            await _imageStorageService.DeleteAsync(storedImage.FileName);
            throw;
        }

        return uploadedImage;
    }

    public async Task<UploadedImage?> GetByIdAsync(int id)
    {
        return await _uploadedImageRepository.GetByIdAsync(id);
    }
    public async Task AddToGalleryAsync(int uploadedImageId)
    {
        var image = await _uploadedImageRepository.GetByIdAsync(uploadedImageId);

        if (image == null)
        {
            throw new ArgumentException(
                "Bilden kunde inte hittas.",
                nameof(uploadedImageId));
        }

        var galleryImages = await _imageUsageRepository.GetGalleryImagesAsync();

        bool alreadyInGallery = galleryImages.Any(
            usage => usage.UploadedImageId == uploadedImageId);

        if (alreadyInGallery)
        {
            return;
        }

        var imageUsage = new ImageUsage
        {
            UploadedImageId = uploadedImageId,
            PostId = null,
            CommentId = null,
            IsInGallery = true,
            SortOrder = galleryImages.Count
        };

        await _imageUsageRepository.AddAsync(imageUsage);
        await _imageUsageRepository.SaveChangesAsync();
    }
    public async Task RemoveFromGalleryAsync(int uploadedImageId)
    {
        var galleryImages = await _imageUsageRepository
            .GetGalleryImagesAsync();

        var galleryImage = galleryImages.FirstOrDefault(
            usage => usage.UploadedImageId == uploadedImageId);

        if (galleryImage == null)
        {
            return;
        }

        _imageUsageRepository.Delete(galleryImage);

        await _imageUsageRepository.SaveChangesAsync();
    }
    public async Task AddToPostAsync(int uploadedImageId, int postId)
    {
        // Kontrollera att bilden finns.
        var image = await _uploadedImageRepository
            .GetByIdAsync(uploadedImageId);

        if (image == null)
        {
            throw new ArgumentException(
                "Bilden kunde inte hittas.",
                nameof(uploadedImageId));
        }

        // Kontrollera att inlägget finns.
        var post = await _postRepository.GetByIdAsync(postId);

        if (post == null)
        {
            throw new ArgumentException(
                "Inlägget kunde inte hittas.",
                nameof(postId));
        }

        // Hämta befintliga bildkopplingar för inlägget.
        var existingUsages =
            await _imageUsageRepository.GetByPostAsync(postId);

        // Förhindra dubbletter.
        if (existingUsages.Any(usage =>
            usage.UploadedImageId == uploadedImageId))
        {
            return;
        }

        // Skapa kopplingen.
        var imageUsage = new ImageUsage
        {
            UploadedImageId = uploadedImageId,
            PostId = postId,
            CommentId = null,
            IsInGallery = false,
            SortOrder = existingUsages.Count
        };

        await _imageUsageRepository.AddAsync(imageUsage);
        await _imageUsageRepository.SaveChangesAsync();
    }
    public async Task AddToCommentAsync(int uploadedImageId, int commentId)
    {
        // Kontrollera att bilden finns.
        var image = await _uploadedImageRepository
            .GetByIdAsync(uploadedImageId);

        if (image == null)
        {
            throw new ArgumentException(
                "Bilden kunde inte hittas.",
                nameof(uploadedImageId));
        }

        // Kontrollera att kommentaren finns.
        var comment = await _commentService.GetByIdAsync(commentId);

        if (comment == null)
        {
            throw new ArgumentException(
                "Kommentaren kunde inte hittas.",
                nameof(commentId));
        }

        // Hämta befintliga bildkopplingar för kommentaren.
        var existingUsages =
            await _imageUsageRepository.GetByCommentAsync(commentId);

        // Förhindra dubbletter.
        if (existingUsages.Any(usage =>
            usage.UploadedImageId == uploadedImageId))
        {
            return;
        }

        // Skapa kopplingen mellan bilden och kommentaren.
        var imageUsage = new ImageUsage
        {
            UploadedImageId = uploadedImageId,
            PostId = null,
            CommentId = commentId,
            IsInGallery = false,
            SortOrder = existingUsages.Count
        };

        await _imageUsageRepository.AddAsync(imageUsage);
        await _imageUsageRepository.SaveChangesAsync();
    }
}
