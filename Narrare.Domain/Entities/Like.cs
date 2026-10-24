using System;
using System.Collections.Generic;
using System.Text;


namespace Narrare.Domain.Entities;

public class Like
{
    public int Id { get; set; }

    // Användaren som har tryckt på blomman.
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // Inlägget som har fått en gilla-markering.
    // Nullable eftersom en Like ska kunna tillhöra antingen ett inlägg eller en kommentar.
    public int? PostId { get; set; }
    public Post? Post { get; set; }

    // Kommentaren som har fått en gilla-markering.
    public int? CommentId { get; set; }
    public Comment? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
