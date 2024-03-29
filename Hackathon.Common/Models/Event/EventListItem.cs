using System;
using Hackathon.Common.Models.Tags;

namespace Hackathon.Common.Models.Event;

/// <summary>
/// Списочное представление события
/// </summary>
public class EventListItem
{
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование события
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание события
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Дата и время начала
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Максимальное количество участников
    /// </summary>
    public int MaxEventMembers { get; set; }

    /// <summary>
    /// Минимальное количество участников в команде
    /// </summary>
    public int MinTeamMembers { get; set; }

    /// <summary>
    /// Статус события
    /// </summary>
    public EventStatus Status { get; set; }

    /// <summary>
    /// Идентификатор организатора
    /// </summary>
    public long OwnerId { get; set; }

    /// <summary>
    /// Наименование организатора
    /// </summary>
    public string OwnerName { get; set; }

    /// <summary>
    /// Количество команд связанных с событием
    /// </summary>
    public long TeamsCount { get; set; }

    /// <summary>
    /// Количество участников
    /// </summary>
    public long MembersCount { get; set; }

    /// <summary>
    /// Создавать команды автоматически
    /// </summary>
    public bool IsCreateTeamsAutomatically { get; set; }

    /// <summary>
    /// Идентификатор изображения
    /// </summary>
    public Guid? ImageId { get; set; }
    
    /// <summary>
    /// Теги
    /// </summary>
    public TagCollection Tags { get; set; }
}
