using System;
using System.Collections.Generic;
using System.Text;


namespace Narrare.Application.DTOs;

public class CreateGroupConversationDto
{
    public string Name { get; set; } = string.Empty;

    public List<int> ParticipantIds { get; set; } = new();
}
