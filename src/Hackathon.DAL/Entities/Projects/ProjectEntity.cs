using System;
using Hackathon.DAL.Entities.Event;
using Hackathon.DAL.Entities.Teams;

namespace Hackathon.DAL.Entities.Projects;

/// <summary>
/// Проект
/// </summary>
public class ProjectEntity
{
    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    public long EventId { get; set; }

    /// <summary>
    /// Мероприятие
    /// </summary>
    public EventEntity Event { get; set; }

    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Команда
    /// </summary>
    public TeamEntity Team { get; set; }

    /// <summary>
    /// Идентификаторы файлов проекта
    /// </summary>
    public Guid[] FileIds { get; set; } = Array.Empty<Guid>();

    /// <summary>
    /// Ссылка на ветку Git-репозитория
    /// </summary>
    public string LinkToGitBranch { get; set; }
}
