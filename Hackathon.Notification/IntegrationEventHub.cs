using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Hackathon.Notification;

public class IntegrationEventHub<TIntegrationEvent>: Hub, IMessageHub<TIntegrationEvent>
where TIntegrationEvent: IIntegrationEvent

{
    private readonly IHubContext<IntegrationEventHub<TIntegrationEvent>> _contextHub;

    public IntegrationEventHub(IHubContext<IntegrationEventHub<TIntegrationEvent>> contextHub)
    {
        _contextHub = contextHub;
    }

    public async Task Publish(string topic, TIntegrationEvent message)
    {
        var serializedObject = JsonSerializer.Serialize(message);
        await _contextHub.Clients.All.SendCoreAsync(topic, new object[]{serializedObject});
    }
}