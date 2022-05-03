using System;

namespace Hackathon.Common.Models.Event
{
    public class EventFilter
    {
        public long[] Ids { get; set; }
        public string Name { get; set; }
        public DateTime? StartFrom { get; set; }
        public DateTime? StartTo { get; set; }
        public EventStatus[] Statuses { get; set; }

        /// <summary>
        /// Исключить события других пользователей в статусе черновик
        /// </summary>
        public bool ExcludeOtherUsersDraftedEvents { get; set; } = true;
        
        /// <summary>
        /// Идентификаторы команд принимавших участие в событии
        /// </summary>
        public long[] TeamsIds { get; set; }
    }
}