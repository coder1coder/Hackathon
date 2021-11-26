﻿using System;
using System.Collections.Generic;
using Hackathon.Common.Models.Team;

namespace Hackathon.Common.Models.Event
{
    public class EventFullModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime StartMemberRegistration { get; set; }
        public EventStatus Status { get; set; }

        public int MinTeamMembers { get; set; }
        public int MaxEventMembers { get; set; }

        public List<TeamModel> Teams { get; set; } = new List<TeamModel>();
    }
}