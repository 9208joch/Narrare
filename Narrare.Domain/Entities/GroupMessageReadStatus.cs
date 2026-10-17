using System;
using System.Collections.Generic;
using System.Text;


namespace Narrare.Domain.Entities;

public class GroupMessageReadStatus
{
    public int Id { get; set; }

    // Vilket gruppmeddelande lässtatusen gäller.
    public int MessageId { get; set; }
    public PrivateMessage? Message { get; set; }

    // Vilken användare lässtatusen gäller.
    public int UserId { get; set; }
    public User? User { get; set; }

    // Om användaren har läst meddelandet.
    public bool IsRead { get; set; }
}
