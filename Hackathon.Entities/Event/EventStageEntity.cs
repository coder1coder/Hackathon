using Hackathon.Entities.Interfaces;

namespace Hackathon.Entities.Event;

/// <summary>
/// Этап события
/// </summary>
public class EventStageEntity: BaseEntity, ISoftDeletable
{
    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Индекс сортировки
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Продолжительность
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }

    /// <summary>
    /// Событие
    /// </summary>
    public EventEntity Event { get; set; }

    /// <summary>
    /// Признак удаленной сущности
    /// </summary>
    public bool IsDeleted { get; set; }
}
