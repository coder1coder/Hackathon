﻿using System;
using Hackathon.Common.Models.Event;

namespace Hackathon.Common.Entities
{
    public class EventEntity: BaseEntity
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public EventStatus Status { get; set; }
    }
}