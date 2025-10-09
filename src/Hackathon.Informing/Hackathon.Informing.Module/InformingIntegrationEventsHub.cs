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
        var topicName = integrationEvent.GetTopicName();

        if (topicName is null)
        {
            return;
        }

        await _contextHub.Clients.All.SendCoreAsync(topicName, [
            integrationEvent
        ]);
    }
}
