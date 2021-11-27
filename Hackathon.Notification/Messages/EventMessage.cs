namespace Hackathon.MessageQueue.Messages
{
    public class EventMessage: IMessage
    {
        public long EventId { get; }
        public string Message { get; }

        public EventMessage(long eventId, string message)
        {
            EventId = eventId;
            Message = message;
        }
    }
}