using System;
using System.Collections.Generic;
using System.Text;


namespace Narrare.Infrastructure.Configuration;

public class ImageStorageOptions
{
    public const string SectionName = "ImageStorage";

    public string Provider { get; set; } = "Local";

    public string LocalPath { get; set; } = "UploadedImages";

    public string ConnectionString { get; set; } = string.Empty;

    public string ContainerName { get; set; } = "narrare-images";
}
