using System.Threading;
using System.Threading.Tasks;

namespace Hackathon.Chats.Abstractions.IntegrationEvents;

public interface ITeamChatHub: IChatsIntegrationEventsHub<ITeamChatIntegrationEvent>
{
    Task AddToGroupAsync(long userId, long teamId, CancellationToken cancellationToken = default);
    Task RemoveFromGroupAsync(long userId, long teamId, CancellationToken cancellationToken = default);
}
