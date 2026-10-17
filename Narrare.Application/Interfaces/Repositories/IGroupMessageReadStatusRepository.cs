using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Repositories;

public interface IGroupMessageReadStatusRepository
{
    Task CreateRangeAsync(
        List<GroupMessageReadStatus> statuses);

    Task<List<GroupMessageReadStatus>> GetByUserIdAsync(
        int userId);

    Task MarkConversationAsReadAsync(
        int conversationId,
        int userId);
}
