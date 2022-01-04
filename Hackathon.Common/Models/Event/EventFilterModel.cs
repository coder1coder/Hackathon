using System;
using Hackathon.Common.Models.Base;

namespace Hackathon.Common.Models.Event
{
    public class EventFilterModel: IFilterModel
    {
        public long[] Ids { get; set; }
        public string Name { get; set; }
        public DateTime? StartFrom { get; set; }
        public DateTime? StartTo { get; set; }
        public EventStatus? Status { get; set; }
    }
}