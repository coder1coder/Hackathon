using System;
using System.Collections.Generic;
using Hackathon.Common.Models.Team;

namespace Hackathon.Common.Models.Event
{
    public class EventModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime StartRegistration { get; set; }
        public EventStatus Status { get; set; }
        public int MinMembers { get; set; }
        public int MaxMembers { get; set; }

        public IReadOnlyCollection<TeamModel> Teams { get; set; } = new List<TeamModel>();
    }
}