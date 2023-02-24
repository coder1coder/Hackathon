using Hackathon.Entities.Event;
using Hackathon.Entities.Interfaces;

namespace Hackathon.Entities;

public class ProjectEntity: BaseEntity, ISoftDeletable
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
    /// Признак удаления
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}