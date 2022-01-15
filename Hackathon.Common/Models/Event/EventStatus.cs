namespace Hackathon.Common.Models.Event
{
    public enum EventStatus
    {
        Draft = default,
        Published = 1,
        Started = 2,
        Development = 3,
        Prepare = 4,
        Presentation = 5,
        Decision = 6,
        Award = 7,
        Finished = 8,
    }
}