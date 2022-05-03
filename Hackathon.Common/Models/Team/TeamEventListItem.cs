namespace Hackathon.Common.Models.Team;

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
    /// Идентификатор проекта
    /// </summary>
    public long ProjectId { get; set; }
    
    /// <summary>
    /// Наименование проекта
    /// </summary>
    public string ProjectName { get; set; }
}