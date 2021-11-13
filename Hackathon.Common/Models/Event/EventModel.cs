using System;

namespace Hackathon.Common.Models.Event
{
    public class EventModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public EventStatus Status { get; set; }
    }
}