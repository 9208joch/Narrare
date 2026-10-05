using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Domain.Entities;

public class Page
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int VisitCount { get; set; }
}