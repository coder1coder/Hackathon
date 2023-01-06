using System.Text.RegularExpressions;
using FluentValidation;
using Hackathon.Common;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Validation.Project;

public class ProjectUpdateFromGitParametersValidator: AbstractValidator<ProjectUpdateFromGitParameters>
{
    public ProjectUpdateFromGitParametersValidator()
    {
        RuleFor(x => x.ProjectId)
            .GreaterThan(default(long))
            .WithMessage("Идентификатор проекта должен быть больше 0");

        RuleFor(x => x.LinkToGitBranch)
            .MustAsync((value, _) => IsGitBranchValid(value))
            .WithMessage("Указана некорректная ссылка на git репозиторий")
            .When(x => x.LinkToGitBranch is not null);
    }

    /// <summary>
    /// Проверка ссылки на репозиторий с указанием ветки
    /// <example>https://github.com/coder1coder/Hackathon/tree/develop</example>
    /// </summary>
    private static async Task<bool> IsGitBranchValid(string linkToGitBranch)
    {
        if (!Regex.IsMatch(linkToGitBranch, RegexPatterns.GithubBranchLink))
            return false;

        return await Task.FromResult(true);
    }
}
