using Microsoft.Extensions.Configuration;
using Narrare.Application.Interfaces.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using Narrare.Infrastructure.Configuration;


namespace Narrare.Infrastructure.Storage;


//public class ImageStorageService : IImageStorageService att fixa för testning
public class LocalImageStorageService : IImageStorageService
{
    public Task<Stream?> OpenReadAsync(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) ||
            Path.GetFileName(fileName) != fileName)
        {
            throw new ArgumentException(
                "Ogiltigt filnamn.",
                nameof(fileName));
        }
        
        string filePath = Path.Combine(_storageDirectory, fileName);

        if (!File.Exists(filePath))
        {
            return Task.FromResult<Stream?>(null);
        }

        Stream stream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read);

        return Task.FromResult<Stream?>(stream);
    }
    public Task DeleteAsync(string fileName)
    {
        // Förhindra att filnamnet används för att nå andra kataloger.
        if (string.IsNullOrWhiteSpace(fileName) ||
            Path.GetFileName(fileName) != fileName)
        {
            throw new ArgumentException(
                "Ogiltigt filnamn.",
                nameof(fileName));
        }
        
        string filePath = Path.Combine(_storageDirectory, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
    private const long MaxFileSize = 20 * 1024 * 1024;

    // Skyddar mot bilder med orimligt stora dimensioner.
    private const int MaxDimension = 10000;
    private const long MaxPixels = 40_000_000;

    private readonly string _storageDirectory;

    //public ImageStorageService(IOptions<ImageStorageOptions> options)  att fixa för testning
    public LocalImageStorageService(IOptions<ImageStorageOptions> options)
    {
        var storageOptions = options.Value;

        _storageDirectory = Path.IsPathRooted(storageOptions.LocalPath)
            ? storageOptions.LocalPath
            : Path.Combine(
                AppContext.BaseDirectory,
                storageOptions.LocalPath);

        Directory.CreateDirectory(_storageDirectory);
    }

    public async Task<StoredImageInfo> SaveAsync(Stream imageStream)
    {
        if (!imageStream.CanRead || !imageStream.CanSeek)
        {
            throw new ArgumentException(
                "Bildströmmen måste vara läsbar och sökbar.",
                nameof(imageStream));
        }

        if (imageStream.Length == 0)
        {
            throw new ArgumentException("Bildfilen är tom.");
        }

        if (imageStream.Length > MaxFileSize)
        {
            throw new ArgumentException(
                "Bilden får inte vara större än 20 MB.");
        }

        imageStream.Position = 0;

        using Image image = await Image.LoadAsync(imageStream);

        if (image.Width > MaxDimension ||
            image.Height > MaxDimension ||
            (long)image.Width * image.Height > MaxPixels)
        {
            throw new ArgumentException(
                "Bildens dimensioner är för stora.");
        }

        string fileName = $"{Guid.NewGuid():N}.jpg";
        string filePath = Path.Combine(_storageDirectory, fileName);

        try
        {
            await using var fileStream = new FileStream(
                filePath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                useAsync: true);

            await image.SaveAsJpegAsync(
                fileStream,
                new JpegEncoder
                {
                    Quality = 85
                });
        }
        catch
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            throw;
        }

        var fileSize = new FileInfo(filePath).Length;

        return new StoredImageInfo(fileName, fileSize);
    }
}

