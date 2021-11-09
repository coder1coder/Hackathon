using System;
using System.ComponentModel.DataAnnotations;

namespace Hackathon.Contracts.Requests
{
    public class CreateEventRequest
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
    }
}