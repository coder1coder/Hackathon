using System.Threading.Tasks;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvents;
using Hackathon.IntegrationEvents.Topics;
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
        var topicName = ResolveTopicName(integrationEvent);

        if (topicName is null)
            return;

        await _contextHub.Clients.All.SendCoreAsync(topicName, new object[]
        {
            integrationEvent
        });
    }
    
    private static string ResolveTopicName<T>(T integrationEvent) where T : IIntegrationEvent
        => integrationEvent switch
        {
            EventStageChangedIntegrationEvent => EventsTopicNames.EventStageChanged,
            EventStatusChangedIntegrationEvent => EventsTopicNames.EventStatusChanged,
            _ => null
        };
}
