using System.Threading.Tasks;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Informing.Abstractions.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace Hackathon.Informing.Module;

public class InformingIntegrationEventsHub: Hub, IInformingIntegrationEventsHub
{
    private readonly IHubContext<InformingIntegrationEventsHub> _contextHub;

    public InformingIntegrationEventsHub(IHubContext<InformingIntegrationEventsHub> contextHub)
    {
        _contextHub = contextHub;
    }

    public async Task PublishAll(IIntegrationEvent integrationEvent)
    {
        var topicName = ResolveTopicName(integrationEvent);

        if (topicName is null)
        {
            return;
        }

        await _contextHub.Clients.All.SendCoreAsync(topicName, new object[]
        {
            integrationEvent
        });
    }

    private static string ResolveTopicName<T>(T integrationEvent) where T : IIntegrationEvent
        => integrationEvent switch
        {
            NotificationChangedIntegrationEvent => InformingTopicNames.NotificationChanged,
            _ => null
        };
}
