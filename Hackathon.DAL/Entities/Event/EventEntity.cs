using Hackathon.Common.Models.Event;
using Hackathon.DAL.Entities.Interfaces;
using Hackathon.DAL.Entities.User;
using System;
using System.Collections.Generic;

namespace Hackathon.DAL.Entities.Event;

/// <summary>
/// Событие
/// </summary>
public class EventEntity : BaseEntity, ISoftDeletable
{
    /// <summary>
    /// Кто создал событие
    /// </summary>
    public long OwnerId { get; set; }

    public UserEntity Owner { get; set; }

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
    /// <remarks>В обязательном порядке сохранять в UTC</remarks>
    public DateTime Start { get; set; }

    /// <summary>
    /// Продолжительность формированя команд в минутах
    /// </summary>
    public int MemberRegistrationMinutes { get; set; }

    /// <summary>
    /// Статус события
    /// </summary>
    public EventStatus Status { get; set; }

    /// <summary>
    /// Продолжительность этапа разработки в минутах
    /// </summary>
    public int DevelopmentMinutes { get; set; }

    /// <summary>
    /// Продолжительность выступленя каждой команды в минутах
    /// </summary>
    public int TeamPresentationMinutes { get; set; }

    /// <summary>
    /// Максимальное количество участников
    /// </summary>
    public int MaxEventMembers { get; set; }

    /// <summary>
    /// Минимальное количество участников в команде
    /// </summary>
    public int MinTeamMembers { get; set; }

    /// <summary>
    /// создавать команды автоматически
    /// </summary>
    public bool IsCreateTeamsAutomatically { get; set; }

    /// <summary>
    /// Список сообщений высылаемых командам при смене статусов
    /// </summary>
    public ICollection<ChangeEventStatusMessage> ChangeEventStatusMessages { get; set; } = new List<ChangeEventStatusMessage>();

    /// <summary>
    /// Команды связанные с событием
    /// </summary>
    public ICollection<TeamEntity> Teams { get; set; } = new List<TeamEntity>();

    /// <summary>
    /// Идентификатор текущего этапа события
    /// </summary>
    public long CurrentStageId { get; set; }

    /// <summary>
    /// Этапы событи
    /// </summary>
    public ICollection<EventStageEntity> Stages { get; set; } = new List<EventStageEntity>();

    /// <summary>
    /// Награда, призовой фонд
    /// </summary>
    public string Award { get; set; }

    /// <summary>
    /// Изображение события
    /// </summary>
    public Guid? ImageId { get; set; }

    /// <summary>
    /// Правила участия в событии
    /// </summary>
    public string Rules { get; set; }

    /// <summary>
    /// Признак удаления
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Задачи, которые ставятся перед участниками мероприятия
    /// </summary>
    public EventTaskItem[] Tasks { get; set; } = Array.Empty<EventTaskItem>();
}
