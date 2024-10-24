﻿using System.Collections.Generic;
using Hackathon.Common.Models.Teams;
using Hackathon.Common.Models.Users;

namespace Hackathon.Common.Models.Event;

public class EventModel : EventUpdateParameters
{
    /// <summary>
    /// Кто создал событие
    /// </summary>
    public long OwnerId { get; set; }

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

    /// <summary>
    /// Соглашение об участии в мероприятии
    /// </summary>
    public new EventAgreementModel Agreement { get; set; }

    /// <summary>
    /// Идентификатор заявки на согласование
    /// </summary>
    public long? ApprovalApplicationId { get; set; }

    /// <summary>
    /// Заявление на согласование
    /// </summary>
    public ApprovalApplicationModel ApprovalApplication { get; set; }
}
