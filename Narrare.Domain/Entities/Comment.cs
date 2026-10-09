using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Domain.Entities;

public class Comment
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Foreign keys
    public int UserId { get; set; }

    public int PostId { get; set; }

    // Navigation properties
    public User? User { get; set; } = null!;

    public Post? Post { get; set; } = null!;
    public bool IsDeleted { get; set; }

    public int? DeletedByUserId { get; set; }

    public DateTime? DeletedAt { get; set; }
}