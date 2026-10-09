using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Domain.Entities;

public class PageContent
{
    public int Id { get; set; }

    public int PageId { get; set; }

    public Page? Page { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int Order { get; set; }
}
