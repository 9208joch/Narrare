
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using Narrare.Application.Interfaces.Services;
using Narrare.Infrastructure.Configuration;
using SkiaSharp;

namespace Narrare.Infrastructure.Storage;

public class AzureImageStorageService : IImageStorageService
{
    private const long MaxFileSize = 20 * 1024 * 1024;
    private const int MaxDimension = 10000;
    private const long MaxPixels = 40_000_000;

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
                "Bildströmmen måste vara läsbar och sökbar.",
                nameof(imageStream));
        }

        if (imageStream.Length <= 0 || imageStream.Length > MaxFileSize)
        {
            throw new ArgumentException(
                "Bilden måste vara större än 0 byte och högst 20 MB.");
        }

        imageStream.Position = 0;

        using var codec = SKCodec.Create(imageStream)
            ?? throw new ArgumentException(
                "Filen innehåller ingen giltig bild.");

        var info = codec.Info;

        if (info.Width <= 0 ||
            info.Height <= 0 ||
            info.Width > MaxDimension ||
            info.Height > MaxDimension ||
            (long)info.Width * info.Height > MaxPixels)
        {
            throw new ArgumentException(
                "Bildens dimensioner är för stora.");
        }

        using var bitmap = SKBitmap.Decode(codec)
            ?? throw new ArgumentException(
                "Bilden kunde inte avkodas.");

        using var image = SKImage.FromBitmap(bitmap);

        using var encoded = image.Encode(
            SKEncodedImageFormat.Jpeg,
            85)
            ?? throw new InvalidOperationException(
                "Bilden kunde inte konverteras till JPEG.");

        var fileName = $"{Guid.NewGuid():N}.jpg";
        var blobClient = _containerClient.GetBlobClient(fileName);

        await using var convertedStream = new MemoryStream();

        encoded.SaveTo(convertedStream);
        convertedStream.Position = 0;

        await blobClient.UploadAsync(
            convertedStream,
            new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = "image/jpeg"
                }
            });

        return new StoredImageInfo(
            fileName,
            convertedStream.Length);
    }

    public async Task<Stream?> OpenReadAsync(string fileName)
    {
        ValidateFileName(fileName);

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
        ValidateFileName(fileName);

        var blobClient = _containerClient.GetBlobClient(fileName);

        await blobClient.DeleteIfExistsAsync();
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
