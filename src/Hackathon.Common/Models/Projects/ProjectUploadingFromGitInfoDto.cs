using System;

namespace Hackathon.Common.Models.Projects;

public class ProjectUploadingFromGitInfoDto: IHasProjectIdentity
{
    /// <summary>
    /// Ссылка на репозиторий с указанием ветки
    /// <example>https://github.com/coder1coder/Hackathon/tree/develop</example>
    /// </summary>
    public string LinkToGitBranch { get; set; }

    /// <summary>
    /// Идентификаторы файлов загруженные из репозитория
    /// </summary>
    public Guid[] FileIds { get; set; }

    public long TeamId { get; set; }
    public long EventId { get; set; }
}
