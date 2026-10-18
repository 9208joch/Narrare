using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Domain.Entities;

public class ContentReport
{
    public int Id { get; set; }

    // Vem som skickade anmälan.
    public int ReporterUserId { get; set; }

    public User? ReporterUser { get; set; }

    // Vilket inlägg eller vilken kommentar anmälan gäller.
    public int? PostId { get; set; }

    public Post? Post { get; set; }

    public int? CommentId { get; set; }

    public Comment? Comment { get; set; }

    // Användarens motivering.
    public string Reason { get; set; } = string.Empty;

    // När anmälan skapades.
    public DateTime CreatedAt { get; set; }

    // Om administratören har granskat anmälan.
    public bool IsReviewed { get; set; }
}