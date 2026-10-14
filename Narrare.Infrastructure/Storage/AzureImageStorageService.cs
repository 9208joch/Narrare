using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using Narrare.Application.Interfaces.Services;
using Narrare.Infrastructure.Configuration;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Infrastructure.Storage;

public class AzureImageStorageService : IImageStorageService
{
    private const long MaxFileSize = 20 * 1024 * 1024;

    private readonly BlobContainerClient _containerClient;

    public AzureImageStorageService(
        IOptions<ImageStorageOptions> options)
    {
        var storageOptions = options.Value;

        if (string.IsNullOrWhiteSpace(storageOptions.ConnectionString))
        {
            throw new InvalidOperationException(
                "Azure Blob Storage saknar ConnectionString.");
        }

        var blobServiceClient =
            new BlobServiceClient(storageOptions.ConnectionString);

        _containerClient = blobServiceClient.GetBlobContainerClient(
            storageOptions.ContainerName);
    }

    public async Task<StoredImageInfo> SaveAsync(Stream imageStream)
    {
        if (!imageStream.CanRead || !imageStream.CanSeek)
        {
            throw new ArgumentException(
                "Bildströmmen måste vara läsbar och sökbar.");
        }

        if (imageStream.Length <= 0 || imageStream.Length > MaxFileSize)
        {
            throw new ArgumentException(
                "Bilden måste vara större än 0 byte och högst 20 MB.");
        }

        imageStream.Position = 0;

        using var image =
            await SixLabors.ImageSharp.Image.LoadAsync(imageStream);

        if (image.Width > 10000 ||
            image.Height > 10000 ||
            (long)image.Width * image.Height > 40_000_000)
        {
            throw new ArgumentException(
                "Bildens dimensioner är för stora.");
        }

        var fileName = $"{Guid.NewGuid():N}.jpg";
        var blobClient = _containerClient.GetBlobClient(fileName);

        await using var convertedStream = new MemoryStream();

        await image.SaveAsJpegAsync(
            convertedStream,
            new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder
            {
                Quality = 85
            });

        convertedStream.Position = 0;

        await blobClient.UploadAsync(
            convertedStream,
            new BlobHttpHeaders
            {
                ContentType = "image/jpeg"
            });

        return new StoredImageInfo(
            fileName,
            convertedStream.Length);
    }
    public async Task<Stream?> OpenReadAsync(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) ||
            Path.GetFileName(fileName) != fileName)
        {
            throw new ArgumentException(
                "Ogiltigt filnamn.",
                nameof(fileName));
        }

        var blobClient = _containerClient.GetBlobClient(fileName);

        if (!await blobClient.ExistsAsync())
        {
            return null;
        }

        var response = await blobClient.DownloadStreamingAsync();

        return response.Value.Content;
    }

    public async Task DeleteAsync(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) ||
            Path.GetFileName(fileName) != fileName)
        {
            throw new ArgumentException(
                "Ogiltigt filnamn.",
                nameof(fileName));
        }

        var blobClient = _containerClient.GetBlobClient(fileName);

        await blobClient.DeleteIfExistsAsync();
    }
}
