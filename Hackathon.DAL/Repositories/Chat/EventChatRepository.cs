using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat.Event;
using StackExchange.Redis;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hackathon.DAL.Repositories.Chat;

public class EventChatRepository: ChatRepository<EventChatMessage>, IEventChatRepository
{
    protected override Expression<Func<EventChatMessage, long>> EntityIdentityKey => x => x.EventId;

    public EventChatRepository(IConnectionMultiplexer connectionMultiplexer): base(connectionMultiplexer)
    {
    }

    public Task<BaseCollection<EventChatMessage>> GetMessagesAsync(long eventId, int offset = 0, int limit = 300)
        => GetMessagesByKeyAsync(eventId.ToString(), offset, limit);
}
