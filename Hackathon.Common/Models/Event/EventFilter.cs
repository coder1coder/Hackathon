using System;

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
    /// Исключить события других пользователей в статусе черновик
    /// </summary>
    public bool ExcludeOtherUsersDraftedEvents { get; set; } = true;

    /// <summary>
    /// Идентификаторы команд принимавших участие в событии
    /// </summary>
    public long[] TeamsIds { get; set; }

    /// <summary>
    /// Идентификаторы пользователей создавших событие
    /// </summary>
    public long[] OwnerIds { get; set; }
}
