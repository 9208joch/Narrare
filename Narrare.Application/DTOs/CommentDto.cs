using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Application.DTOs;

public class CommentDto
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int UserId { get; set; }

    public int PostId { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedByUserId { get; set; }

    public DateTime? DeletedAt { get; set; }
}