namespace Hackathon.Common.Models.EventStage;

/// <summary>
/// Этап события
/// </summary>
/// <remarks>Не добавлять ссылку на Event, при маппинге будут проблемы</remarks>
public class EventStageModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Продолжительность в минутах
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Индекс сортировки
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }
}
