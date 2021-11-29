using System;

namespace Hackathon.Common.Models.Event
{
    public class CreateEventModel
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime StartMemberRegistration { get; set; }
        public int MemberRegistrationMinutes { get; set; } = 30;
        public int MinTeamMembers { get; set; }
        public int MaxEventMembers { get; set; }
    }
}