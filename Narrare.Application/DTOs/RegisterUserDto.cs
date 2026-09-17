using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Application.DTOs;

public class RegisterUserDto
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int Age { get; set; }

    public string Location { get; set; } = string.Empty;

    public string? Occupation { get; set; }

    public string? ProfileImageUrl { get; set; }
}