using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Narrare.Domain.Entities;

public class Post
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Foreign keys
    public int UserId { get; set; }

    public int CategoryId { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;

    public Category Category { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}