using System;
using System.Collections.Generic;
using System.Text;


namespace Narrare.Application.DTOs;

public class CreatePrivateMessageDto
{
    // ID för användaren som ska ta emot meddelandet.
    public int ReceiverId { get; set; }

    // Själva meddelandet.
    public string Content { get; set; } = string.Empty;
}
