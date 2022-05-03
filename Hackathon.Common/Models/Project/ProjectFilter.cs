namespace Hackathon.Common.Models.Project;

/// <summary>
/// Фильтр для получения списка проектов
/// </summary>
public class ProjectFilter
{
    /// <summary>
    /// Идентификаторы проектов
    /// </summary>
    public long[] Ids { get; set; }
    
    /// <summary>
    /// Идентификаторы событий
    /// </summary>
    public long[] EventsIds { get; set; }
    
    /// <summary>
    /// Идентификаторы команд
    /// </summary>
    public long[] TeamsIds { get; set; }
}