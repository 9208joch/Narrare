using System;
using System.Collections.Generic;
using System.Text;


namespace Narrare.Application.Interfaces.Services;

public interface IImageStorageService
{
    Task<StoredImageInfo> SaveAsync(Stream imageStream);
    Task<Stream?> OpenReadAsync(string fileName);

    Task DeleteAsync(string fileName);
}

public record StoredImageInfo(
    string FileName,
    long FileSize);
