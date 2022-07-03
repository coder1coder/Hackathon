using System.Collections.Generic;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.Event
{
    public class EventModel : EventUpdateParameters
    {
        /// <summary>
        /// Этапы
        /// </summary>
        public List<EventStage> Stages { get; set; } = new();

        /// <summary>
        /// Статус события
        /// </summary>
        public EventStatus Status { get; set; }

        /// <summary>
        /// Организатор
        /// </summary>
        public UserModel Owner { get; set; }

        /// <summary>
        /// Список команд связанных с событием
        /// </summary>
        public List<TeamModel> Teams { get; set; } = new();
    }
}
