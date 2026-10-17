using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Domain.Entities;

public class ConversationParticipant
{
    public int Id { get; set; }

    // Vilken konversation deltagaren tillhör.
    public int ConversationId { get; set; }
    public Conversation? Conversation { get; set; }

    // Vilken användare som deltar.
    public int UserId { get; set; }
    public User? User { get; set; }

    // När användaren gick med i konversationen.
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}