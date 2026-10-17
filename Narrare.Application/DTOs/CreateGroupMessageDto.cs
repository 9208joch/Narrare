using System;
using System.Collections.Generic;
using System.Text;


namespace Narrare.Application.DTOs;

public class CreateGroupMessageDto
{
    public int ConversationId { get; set; }
    public string Content { get; set; } = string.Empty;
}
