namespace Hackathon.Common.Models.Project;

/// <summary>
/// Параметры создания проекта
/// </summary>
public class ProjectCreateParameters
{
    /// <summary>
    /// Наименование проекта
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание проекта
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }
}
