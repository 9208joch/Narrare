using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Domain.Entities;

public class ImageUsage
{
    public int Id { get; set; }

    // Bilden som används.
    public int UploadedImageId { get; set; }
    public UploadedImage? UploadedImage { get; set; }

    // Koppling till ett inlägg, om bilden används där.
    public int? PostId { get; set; }
    public Post? Post { get; set; }

    // Koppling till en kommentar, om bilden används där.
    public int? CommentId { get; set; }
    public Comment? Comment { get; set; }

    // True om bilden ska visas i startsidans galleri.
    public bool IsInGallery { get; set; }

    // Ordningen bilden ska visas i.
    public int SortOrder { get; set; }
}
