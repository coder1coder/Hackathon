namespace Hackathon.Common.Models.Projects;

/// <summary>
/// Списочное представление проекта
/// </summary>
public class ProjectListItem
{
    /// <summary>
    /// Наименование проекта
    /// </summary>
    public string Name { get; set; }

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
}
