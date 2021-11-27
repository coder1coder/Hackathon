using System.Threading.Tasks;
using Hackathon.MessageQueue.Messages;
using Microsoft.AspNetCore.SignalR;

namespace Hackathon.MessageQueue.Hubs
{
    public class EventMessageHub: Hub, IMessageHub<EventMessage>
    {
        private readonly IHubContext<EventMessageHub> _contextHub;

        public EventMessageHub(IHubContext<EventMessageHub> contextHub)
        {
            _contextHub = contextHub;
        }
        public async Task Publish(string topic, EventMessage message)
        {
            await _contextHub.Clients.All.SendCoreAsync(topic, new object[] { message });
        }
    }
}