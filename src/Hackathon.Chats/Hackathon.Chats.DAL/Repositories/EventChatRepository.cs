using Hackathon.Chats.Abstractions.Models;
using Hackathon.Chats.Abstractions.Models.Events;
using Hackathon.Chats.Abstractions.Repositories;
using MapsterMapper;

namespace Hackathon.Chats.DAL.Repositories;

public sealed class EventChatRepository: BaseChatRepository<EventChatMessage>, IEventChatRepository
{
    public EventChatRepository(ChatsDbContext dbContext, IMapper mapper) : base(dbContext, mapper, ChatType.EventChat)
    {
    }
}
