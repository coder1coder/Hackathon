using System;

namespace Hackathon.Contracts.Requests.Event
{
    public class CreateEventRequest
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime StartMemberRegistration { get; set; }
        public int MinTeamMembers { get; set; }
        public int MaxEventMembers { get; set; }
    }
}