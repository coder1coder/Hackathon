using System;

namespace Hackathon.Contracts.Requests.Event
{
    public class CreateEventRequest
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
    }
}