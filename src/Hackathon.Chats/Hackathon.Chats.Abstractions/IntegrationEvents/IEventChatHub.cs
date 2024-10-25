using System.Threading;
using System.Threading.Tasks;

namespace Hackathon.Chats.Abstractions.IntegrationEvents;

public interface IEventChatHub: IChatsIntegrationEventsHub<IEventChatIntegrationEvent>
{
    Task AddToGroupAsync(long userId, long eventId, CancellationToken cancellationToken = default);
    Task RemoveFromGroupAsync(long userId, long eventId, CancellationToken cancellationToken = default);
}
