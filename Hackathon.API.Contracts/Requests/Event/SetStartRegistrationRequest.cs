using System;

namespace Hackathon.Contracts.Requests.Event
{
    public class SetStartRegistrationRequest
    {
        public long Id { get; set; }
        public DateTime StartRegistration { get; set; }
    }
}