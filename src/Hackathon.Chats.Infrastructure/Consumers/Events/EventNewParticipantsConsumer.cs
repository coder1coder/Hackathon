using System.Threading.Tasks;
using Hackathon.Chats.Abstractions.IntegrationEvents;
using Hackathon.Common.Messages.Events;
using MassTransit;

namespace Hackathon.Chats.Infrastructure.Consumers.Events;

public class EventNewParticipantsConsumer: IConsumer<EventNewParticipantsMessage>
{
    private readonly IEventChatHub _eventChatHub;

    public EventNewParticipantsConsumer(IEventChatHub eventChatHub)
    {
        _eventChatHub = eventChatHub;
    }

    public async Task Consume(ConsumeContext<EventNewParticipantsMessage> context)
    {
        foreach (var participantId in context.Message.ParticipantIds)
        {
            await _eventChatHub.AddToGroupAsync(participantId, context.Message.EventId);
        }
    }
}
