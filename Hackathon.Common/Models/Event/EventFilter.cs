using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hackathon.Common.Models.Event;

/// <summary>
/// Фильтр мероприятий
/// </summary>
public class EventFilter
{
    /// <summary>
    /// Идентификаторы мероприятий
    /// </summary>
    public long[] Ids { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Дата начала
    /// </summary>
    public DateTime? StartFrom { get; set; }

    /// <summary>
    /// Дата окончания
    /// </summary>
    public DateTime? StartTo { get; set; }

    /// <summary>
    /// Статусы
    /// </summary>
    public EventStatus[] Statuses { get; set; }

    /// <summary>
    /// Статусы по которым мероприятия других пользователей будут исключены из результатов
    /// </summary>
    [NotMapped]
    public EventStatus[] ExcludeOtherUsersEventsByStatuses { get; set; }

    /// <summary>
    /// Идентификаторы команд принимавших участие в событии
    /// </summary>
    public long[] TeamsIds { get; set; }

    /// <summary>
    /// Идентификаторы пользователей создавших событие
    /// </summary>
    public long[] OwnerIds { get; set; }
}
