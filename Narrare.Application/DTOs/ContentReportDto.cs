using System;
using System.Collections.Generic;
using System.Text;


namespace Narrare.Application.DTOs;

public class ContentReportDto
{
    public int Id { get; set; }

    public int ReporterUserId { get; set; }

    public string? ReporterUsername { get; set; }

    public int? PostId { get; set; }

    public string? PostTitle { get; set; }

    public int? CommentId { get; set; }

    public string? CommentContent { get; set; }

    public string Reason { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public bool IsReviewed { get; set; }
    public bool PostIsDeleted { get; set; }
    public bool CommentIsDeleted { get; set; }
}
