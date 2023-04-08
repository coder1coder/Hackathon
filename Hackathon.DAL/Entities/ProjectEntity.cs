using Hackathon.DAL.Entities.Event;
using System;

namespace Hackathon.DAL.Entities;

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
    /// Идентификатор ивента
    /// </summary>
    public long EventId { get; set; }

    public EventEntity Event { get; set; }

    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }

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
