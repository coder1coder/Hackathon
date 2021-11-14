using System;

namespace Hackathon.Common.Models.Event
{
    public class EventFilterModel
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartFrom { get; set; }
        public DateTime? StartTo { get; set; }
        public EventStatus? Status { get; set; }
    }
}