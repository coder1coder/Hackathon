using FluentValidation;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Validation.Projects;

public class UpdateProjectFromGitParametersValidator: AbstractValidator<UpdateProjectFromGitBranchParameters>
{
    public UpdateProjectFromGitParametersValidator(IValidator<IHasProjectIdentity> projectIdentityValidator)
    {
        Include(projectIdentityValidator);

        RuleFor(x => x.LinkToGitBranch)
            .Must(LinkToGitBranchValidator.IsGitBranchValid)
            .When(x=> x.LinkToGitBranch is not null)
            .WithMessage(ProjectValidationErrorMessages.IncorrectGitBranch);
    }
}
