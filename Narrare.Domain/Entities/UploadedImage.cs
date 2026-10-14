
namespace Narrare.Domain.Entities;

public class UploadedImage
{
    public int Id { get; set; }

    // Filnamn som genereras av servern.
    public string FileName { get; set; } = string.Empty;

    // Relativ sökväg till den lagrade bildfilen.
    public string StoragePath { get; set; } = string.Empty;

    // Bildens verifierade MIME-typ, exempelvis image/jpeg.
    public string ContentType { get; set; } = string.Empty;

    // Bildens storlek i byte.
    public long FileSize { get; set; }

    // När bilden laddades upp.
    public DateTime UploadedAt { get; set; }

    // Användaren som laddade upp bilden.
    public int UploadedByUserId { get; set; }

    public User? UploadedByUser { get; set; }
}
