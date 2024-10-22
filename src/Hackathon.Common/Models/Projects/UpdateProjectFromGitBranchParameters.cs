namespace Hackathon.Common.Models.Projects;

/// <summary>
/// Параметры для обновления проекта по ссылке на репозиторий
/// </summary>
public class UpdateProjectFromGitBranchParameters: IHasProjectIdentity
{
    /// <summary>
    /// Ссылка на репозиторий с указанием ветки
    /// <example>https://github.com/coder1coder/Hackathon/tree/develop</example>
    /// </summary>
    public string LinkToGitBranch { get; set; }

    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }
}
