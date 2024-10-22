using System.Threading.Tasks;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace Hackathon.IntegrationEvents;

public class IntegrationEventsHub<TIntegrationEvent>: Hub, IIntegrationEventsHub<TIntegrationEvent>
where TIntegrationEvent: IIntegrationEvent

{
    private readonly IHubContext<IntegrationEventsHub<TIntegrationEvent>> _contextHub;

    public IntegrationEventsHub(IHubContext<IntegrationEventsHub<TIntegrationEvent>> contextHub)
    {
        _contextHub = contextHub;
    }

    public Task PublishAll(string topic, TIntegrationEvent message)
        => _contextHub.Clients.All.SendCoreAsync(topic, new object[]{ message });
}
