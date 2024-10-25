namespace Hackathon.Common.Messages.Events;

public record EventNewParticipantsMessage
{
    public EventNewParticipantsMessage(long eventId, long[] participantIds)
    {
        ParticipantIds = participantIds;
        EventId = eventId;
    }

    public long EventId { get; }
    public long[] ParticipantIds { get; }
}
