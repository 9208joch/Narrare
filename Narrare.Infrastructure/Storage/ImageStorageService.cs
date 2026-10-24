
using Microsoft.Extensions.Options;
using Narrare.Application.Interfaces.Services;
using Narrare.Infrastructure.Configuration;
using SkiaSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Narrare.Infrastructure.Storage;

public class LocalImageStorageService : IImageStorageService
{
    private const long MaxFileSize = 20 * 1024 * 1024;
    private const int MaxDimension = 10000;
    private const long MaxPixels = 40_000_000;

    private readonly string _storageDirectory;

    public LocalImageStorageService(IOptions<ImageStorageOptions> options)
    {
        var storageOptions = options.Value;

        _storageDirectory = Path.IsPathRooted(storageOptions.LocalPath)
            ? storageOptions.LocalPath
            : Path.Combine(AppContext.BaseDirectory, storageOptions.LocalPath);

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
            throw new ArgumentException("Bilden får inte vara större än 20 MB.");
        }

        imageStream.Position = 0;

 
        using var codec = SKCodec.Create(imageStream)
            ?? throw new ArgumentException("Filen innehåller ingen giltig bild.");

        var info = codec.Info;

        if (info.Width <= 0 ||
            info.Height <= 0 ||
            info.Width > MaxDimension ||
            info.Height > MaxDimension ||
            (long)info.Width * info.Height > MaxPixels)
        {
            throw new ArgumentException("Bildens dimensioner är för stora.");
        }

        using var bitmap = SKBitmap.Decode(codec)
            ?? throw new ArgumentException("Bilden kunde inte avkodas.");

        string fileName = $"{Guid.NewGuid():N}.jpg";
        string filePath = Path.Combine(_storageDirectory, fileName);

        try
        {
            using var image = SKImage.FromBitmap(bitmap);
            using var encoded = image.Encode(SKEncodedImageFormat.Jpeg, 85)
                ?? throw new InvalidOperationException(
                    "Bilden kunde inte konverteras till JPEG.");

            await using (var fileStream = new FileStream(
                filePath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                useAsync: true))
            {
                encoded.SaveTo(fileStream);
                await fileStream.FlushAsync();
            }
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

    public Task<Stream?> OpenReadAsync(string fileName)
    {
        ValidateFileName(fileName);

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
        ValidateFileName(fileName);

        string filePath = Path.Combine(_storageDirectory, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }

    private static void ValidateFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) ||
            Path.GetFileName(fileName) != fileName)
        {
            throw new ArgumentException(
                "Ogiltigt filnamn.",
                nameof(fileName));
        }
    }
}
