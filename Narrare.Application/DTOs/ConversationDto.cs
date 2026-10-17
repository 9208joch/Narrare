using System;
using System.Collections.Generic;
using System.Text;


namespace Narrare.Application.DTOs;

public class ConversationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsGroup { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<string> MemberUsernames { get; set; } = new();
}