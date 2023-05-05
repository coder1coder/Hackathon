using Hackathon.Common;
using System.Text.RegularExpressions;

namespace Hackathon.BL.Validation.Project;

public static class LinkToGitBranchValidator
{
    /// <summary>
    /// Проверка ссылки на репозиторий с указанием ветки
    /// <example>https://github.com/coder1coder/Hackathon/tree/develop</example>
    /// </summary>
    public static bool IsGitBranchValid(string linkToGitBranch)
        => Regex.IsMatch(linkToGitBranch, RegexPatterns.GithubBranchLink);
}
