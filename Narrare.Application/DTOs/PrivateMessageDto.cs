using System;
using System.Collections.Generic;
using System.Text;


namespace Narrare.Application.DTOs;

public class PrivateMessageDto
{
    public int Id { get; set; }

    public int SenderId { get; set; }

    public string SenderUsername { get; set; } = string.Empty;

    public int ReceiverId { get; set; }

    public string ReceiverUsername { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime SentAt { get; set; }

    public bool IsRead { get; set; }
}
