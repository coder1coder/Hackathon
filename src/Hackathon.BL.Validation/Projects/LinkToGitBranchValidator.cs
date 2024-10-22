using System.Text.RegularExpressions;
using Hackathon.Common;

namespace Hackathon.BL.Validation.Projects;

public static class LinkToGitBranchValidator
{
    /// <summary>
    /// Проверка ссылки на репозиторий с указанием ветки
    /// <example>https://github.com/coder1coder/Hackathon/tree/develop</example>
    /// </summary>
    public static bool IsGitBranchValid(string linkToGitBranch)
        => Regex.IsMatch(linkToGitBranch, RegexPatterns.GithubBranchLink);
}
