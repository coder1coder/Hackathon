namespace Hackathon.Common.Messages.Events;

public record EventStartedMessage
{
    public long EventId { get; }
    public long[] Participants { get; }
    
    public EventStartedMessage(long eventId, long[] participants)
    {
        EventId = eventId;
        Participants = participants;
    }
}
