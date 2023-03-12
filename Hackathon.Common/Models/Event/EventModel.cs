using System.Collections.Generic;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.Event;

public class EventModel : EventUpdateParameters
{
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

    /// <summary>
    /// Идентификатор текущего этапа события
    /// </summary>
    public long CurrentStageId { get; set; }
}
