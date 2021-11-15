using System;
using System.Collections.Generic;
using Hackathon.Common.Models.Event;

namespace Hackathon.Common.Entities
{
    public class EventEntity: BaseEntity
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime StartRegistration { get; set; }
        public EventStatus Status { get; set; }
        
        public int MinTeamMembers { get; set; }
        public int MaxEventMembers { get; set; }

        public ICollection<TeamEntity> Teams { get; set; }
    }
}