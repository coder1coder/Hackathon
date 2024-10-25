using System.Threading;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.IntegrationEvents;

namespace Hackathon.Chats.Abstractions.IntegrationEvents;

public interface IChatsIntegrationEventsHub<in TChatIntegrationEvent> : IIntegrationEventsHub
where TChatIntegrationEvent: IIntegrationEvent
{
    Task SendEventAsync(TChatIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}
