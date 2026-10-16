using System;
using System.Collections.Generic;
using System.Text;


namespace Narrare.Domain.Entities;

public class PrivateMessage
{
    public int Id { get; set; }

    // Användaren som skickade meddelandet.
    public int SenderId { get; set; }
    public User? Sender { get; set; }

    // Användaren som ska ta emot meddelandet.
    public int ReceiverId { get; set; }
    public User? Receiver { get; set; }

    // Själva meddelandet.
    public string Content { get; set; } = string.Empty;

    // När meddelandet skickades.
    public DateTime SentAt { get; set; }

    // Om mottagaren har läst meddelandet.
    public bool IsRead { get; set; }
}
