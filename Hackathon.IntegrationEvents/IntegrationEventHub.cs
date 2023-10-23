using System.Threading.Tasks;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace Hackathon.IntegrationEvents;

public class IntegrationEventHub<TIntegrationEvent>: Hub, IMessageHub<TIntegrationEvent>
where TIntegrationEvent: IIntegrationEvent

{
    private readonly IHubContext<IntegrationEventHub<TIntegrationEvent>> _contextHub;

    public IntegrationEventHub(IHubContext<IntegrationEventHub<TIntegrationEvent>> contextHub)
    {
        _contextHub = contextHub;
    }

    public Task Publish(string topic, TIntegrationEvent message)
        => _contextHub.Clients.All.SendCoreAsync(topic, new object[]{ message });
}
