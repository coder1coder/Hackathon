using System;
using System.Collections.Generic;
using Hackathon.Common.Models.EventStage;

namespace Hackathon.Common.Models.Event;

public abstract class BaseEventParameters
{
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
    /// Продолжительность формированя команд в минутах
    /// </summary>
    public int MemberRegistrationMinutes { get; set; }

    /// <summary>
    /// Продолжительность этапа разработки в минутах
    /// </summary>
    public int DevelopmentMinutes { get; set; }

    /// <summary>
    /// Продолжительность выступления каждой команды в минутах
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
    public IList<ChangeEventStatusMessage> ChangeEventStatusMessages { get; set; }

    /// <summary>
    /// Этапы события
    /// </summary>
    public List<EventStageModel> Stages { get; set; } = new();

    /// <summary>
    /// Награда, призовой фонд
    /// </summary>
    public string Award { get; set; }

    /// <summary>
    /// Идентификатор изображения ивента
    /// <remarks>Получается посредством загрузки файла в хранилище</remarks>
    /// </summary>
    public Guid? ImageId { get; set; }
}
