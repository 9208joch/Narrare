using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Application.DTOs;

public class UpdateProfileDto
{
    public int Age { get; set; }

    public string Location { get; set; } = string.Empty;

    public string? Occupation { get; set; }

    public string? ProfileImageUrl { get; set; }

    public bool ShowAge { get; set; }

    public bool ShowLocation { get; set; }

    public bool ShowOccupation { get; set; }
}