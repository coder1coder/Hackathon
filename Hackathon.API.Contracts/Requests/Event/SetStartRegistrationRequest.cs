using System;

namespace Hackathon.Contracts.Requests.Event
{
    public class SetStartMemberRegistrationRequest
    {
        public long Id { get; set; }
        public DateTime StartMemberRegistration { get; set; }
    }
}