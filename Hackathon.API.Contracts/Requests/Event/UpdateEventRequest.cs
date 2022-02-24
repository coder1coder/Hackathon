using System;
using System.Collections.Generic;
using Hackathon.Common.Models.Event;

namespace Hackathon.Contracts.Requests.Event
{
    /// <summary>
    /// Контракт создания и обновления нового события
    /// </summary>
    public class UpdateEventRequest: CreateEventRequest
    {
        public long Id { get; set; }
        public long UserId { get; set; }
    }
}