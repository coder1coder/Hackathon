using System.Threading.Tasks;
using Hackathon.Chats.Abstractions.IntegrationEvents;
using Hackathon.Common.Messages.Events;
using MassTransit;

namespace Hackathon.Chats.Infrastructure.Consumers.Events;

public class EventParticipantsLeaveConsumer: IConsumer<EventParticipantsLeaveMessage>
{
    private readonly IEventChatHub _eventChatHub;

    public EventParticipantsLeaveConsumer(IEventChatHub eventChatHub)
    {
        _eventChatHub = eventChatHub;
    }

    public async Task Consume(ConsumeContext<EventParticipantsLeaveMessage> context)
    {
        foreach (var participantId in context.Message.ParticipantIds)
        {
            await _eventChatHub.RemoveFromGroupAsync(participantId, context.Message.EventId);
        }
    }
}
