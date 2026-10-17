using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Domain.Entities;

public class Conversation
{
    public int Id { get; set; }

    // Chattens namn
    public string Name { get; set; } = string.Empty;
    public bool IsGroup { get; set; }

    // När konversationen skapades.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<PrivateMessage> Messages { get; set; }
    = new List<PrivateMessage>();
    public ICollection<ConversationParticipant> Participants { get; set; }
    = new List<ConversationParticipant>();
}