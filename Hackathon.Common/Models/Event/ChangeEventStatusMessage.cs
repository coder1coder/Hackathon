namespace Hackathon.Common.Models.Event
{
    public class ChangeEventStatusMessage
    {
        public EventStatus Status { get; set; }
        public string Message { get; set; }
    }
}