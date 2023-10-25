using Hackathon.Chats.Abstractions.Models.Events;

namespace Hackathon.Chats.Abstractions.Repositories;

public interface IEventChatRepository: IChatRepository<EventChatMessage>
{
}
