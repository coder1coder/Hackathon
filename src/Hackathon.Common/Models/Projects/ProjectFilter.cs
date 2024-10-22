namespace Hackathon.Common.Models.Projects;

/// <summary>
/// Фильтр для получения списка проектов
/// </summary>
public class ProjectFilter
{
    /// <summary>
    /// Идентификаторы событий
    /// </summary>
    public long[] EventsIds { get; set; }

    /// <summary>
    /// Идентификаторы команд
    /// </summary>
    public long[] TeamsIds { get; set; }
}
