using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Application.DTOs;

public class MenuItemDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public int SortOrder { get; set; }
}