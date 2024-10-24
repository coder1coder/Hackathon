using System;
using System.Collections.Generic;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Event.Agreement;
using Hackathon.Common.Models.EventStage;
using Hackathon.Common.Models.Tags;

namespace Hackathon.API.Contracts.Events;

/// <summary>
/// Контракт создания и обновления нового события
/// </summary>
public class UpdateEventRequest
{
    /// <summary>
    /// Идентификатор обновляемой записи
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
    /// Создавать команды автоматически
    /// </summary>
    public bool IsCreateTeamsAutomatically { get; set; }

    /// <summary>
    /// Список сообщений высылаемых командам при смене статусов
    /// </summary>
    public List<ChangeEventStatusMessage> ChangeEventStatusMessages { get; set; }

    /// <summary>
    /// Этапы события
    /// </summary>
    public ICollection<EventStageModel> Stages { get; set; }

    /// <summary>
    /// Награда, призовой фонд
    /// </summary>
    public string Award { get; set; }

    /// <summary>
    /// Идентификатор изображения ивента
    /// <remarks>Получается посредством загрузки файла в хранилище</remarks>
    /// </summary>
    public Guid? ImageId { get; set; }

    /// <summary>
    /// Соглашение об участии в мероприятии
    /// </summary>
    public EventAgreementUpdateParameters Agreement { get; set; }

    /// <summary>
    /// Задачи, которые ставятся перед участниками мероприятия
    /// </summary>
    public EventTaskItem[] Tasks { get; set; }

    /// <summary>
    /// Теги
    /// </summary>
    public TagCollection Tags { get; set; }
}
