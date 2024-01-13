using System.Threading.Tasks;
using Hackathon.Chats.Abstractions.IntegrationEvents;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace Hackathon.Chats.Module;

public class ChatsIntegrationEventsHub: Hub, IChatsIntegrationEventsHub
{
    private readonly IHubContext<ChatsIntegrationEventsHub> _contextHub;

    public ChatsIntegrationEventsHub(IHubContext<ChatsIntegrationEventsHub> contextHub)
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
            EventChatNewMessageIntegrationEvent => ChatsTopicNames.EventChatNewMessage,
            TeamChatNewMessageIntegrationEvent => ChatsTopicNames.TeamChatNewMessage,
            _ => null
        };
}
