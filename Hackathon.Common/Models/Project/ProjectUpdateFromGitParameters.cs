namespace Hackathon.Common.Models.Project;

/// <summary>
/// Параметр обновления проекта из git репозитория
/// </summary>
public class ProjectUpdateFromGitParameters
{
    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public long ProjectId { get; set; }

    /// <summary>
    /// Ссылка на репозиторий с указанием ветки
    /// <example>https://github.com/coder1coder/Hackathon/tree/develop</example>
    /// </summary>
    public string LinkToGitBranch { get; set; }
}
