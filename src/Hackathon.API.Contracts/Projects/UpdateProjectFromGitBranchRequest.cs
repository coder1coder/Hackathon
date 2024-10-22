using Hackathon.Common.Models.Projects;

namespace Hackathon.API.Contracts.Projects;

public class UpdateProjectFromGitBranchRequest: IHasProjectIdentity
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
