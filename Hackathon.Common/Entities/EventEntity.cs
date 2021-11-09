using System;

namespace Hackathon.Common.Entities
{
    public class EventEntity: BaseEntity
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
    }
}