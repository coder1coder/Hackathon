using System.Threading.Tasks;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace Hackathon.IntegrationEvents.Hubs;

public class EventChangesIntegrationEventsHub: Hub, IEventChangesIntegrationEventsHub
{
    private readonly IHubContext<EventChangesIntegrationEventsHub> _contextHub;

    public EventChangesIntegrationEventsHub(IHubContext<EventChangesIntegrationEventsHub> contextHub)
    {
        _contextHub = contextHub;
    }

    public async Task PublishAll(IIntegrationEvent integrationEvent)
    {
        var topicName = integrationEvent.GetTopicName();

        if (topicName is null)
        {
            return;
        }

        await _contextHub.Clients.All.SendCoreAsync(topicName, [ integrationEvent ]);
    }
}
