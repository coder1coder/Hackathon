using Hackathon.Chats.Abstractions.Models.Events;

namespace Hackathon.Chats.Abstractions.Repositories;

/// <summary>
/// Репозиторий сообщений чата мероприятия
/// </summary>
public interface IEventChatRepository: IChatRepository<EventChatMessage>
{
}
