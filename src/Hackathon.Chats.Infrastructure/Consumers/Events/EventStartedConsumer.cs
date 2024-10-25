using System.Threading.Tasks;
using Hackathon.Chats.Abstractions.IntegrationEvents;
using Hackathon.Common.Messages.Events;
using MassTransit;

namespace Hackathon.Chats.Infrastructure.Consumers.Events;

public class EventStartedConsumer: IConsumer<EventStartedMessage>
{
    private readonly IEventChatHub _eventChatHub;

    public EventStartedConsumer(IEventChatHub eventChatHub)
    {
        _eventChatHub = eventChatHub;
    }

    public async Task Consume(ConsumeContext<EventStartedMessage> context)
    {
        foreach (var participant in context.Message.Participants)
        {
            await _eventChatHub.AddToGroupAsync(participant, context.Message.EventId);
        }
    }
}
