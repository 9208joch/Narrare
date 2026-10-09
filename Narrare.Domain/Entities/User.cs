using System;
using System.Collections.Generic;
using Narrare.Domain.Enums;

namespace Narrare.Domain.Entities;

public class User
{
    public int Id { get; set; }

    // Konto
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // Personlig information
    public int Age { get; set; }
    public string Location { get; set; } = string.Empty;
    public string? Occupation { get; set; }

    // Profil
    public string? ProfileImageUrl { get; set; }

    // Användaren bestämmer vad som visas offentligt
    public bool ShowAge { get; set; }
    public bool ShowLocation { get; set; }
    public bool ShowOccupation { get; set; }

    // Konto/status
    public DateTime CreatedAt { get; set; }
    public bool IsBlocked { get; set; }
    public UserRole Role { get; set; }

    // Navigation properties
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}