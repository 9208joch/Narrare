using System;
using System.Collections.Generic;
using System.Text;


namespace Narrare.Application.DTOs;

public class AddConversationParticipantsDto
{
    public List<int> ParticipantIds { get; set; } = new();
}
