namespace Hackathon.Common.Messages.Events;

public record EventParticipantsLeaveMessage
{
    public EventParticipantsLeaveMessage(long eventId, long[] participantIds)
    {
        EventId = eventId;
        ParticipantIds = participantIds;
    }

    public long EventId { get; }
    public long[] ParticipantIds { get; }
}
