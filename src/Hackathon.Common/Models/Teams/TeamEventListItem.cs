namespace Hackathon.Common.Models.Teams;

/// <summary>
/// Списочное представление связи команды с событием
/// </summary>
public class TeamEventListItem
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Наименование команды
    /// </summary>
    public string TeamName { get; set; }

    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }

    /// <summary>
    /// Наименование события
    /// </summary>
    public string EventName { get; set; }

    /// <summary>
    /// Наименование проекта
    /// </summary>
    public string ProjectName { get; set; }
}
