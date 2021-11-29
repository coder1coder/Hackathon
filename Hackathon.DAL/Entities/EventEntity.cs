using System;
using System.Collections.Generic;
using Hackathon.Common.Models.Event;

namespace Hackathon.DAL.Entities
{
    public class EventEntity: BaseEntity
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime StartMemberRegistration { get; set; }

        /// <summary>
        /// Время в минутах выделеное на регистрацию участников
        /// </summary>
        public int MemberRegistrationMinutes { get; set; }
        public EventStatus Status { get; set; }

        public int MinTeamMembers { get; set; }
        public int MaxEventMembers { get; set; }

        public ICollection<TeamEntity> Teams { get; set; }
    }
}