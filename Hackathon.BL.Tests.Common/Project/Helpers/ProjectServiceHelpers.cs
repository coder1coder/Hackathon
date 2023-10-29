using FluentValidation;
using Hackathon.BL.Validation.Project;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Tests.Common.Project.Helpers;

public static class ProjectServiceHelpers
{
    public static IValidator<UpdateProjectFromGitBranchParameters> CreateValidator_UpdateProjectFromGitBranchParameters()
    {
        IValidator<IHasProjectIdentity> iHasProjectIdentityValidator = new ProjectIdentityParametersValidator();
        IValidator<UpdateProjectFromGitBranchParameters> updateProjectFromGitParametersValidator = 
            new UpdateProjectFromGitParametersValidator(iHasProjectIdentityValidator);

        return updateProjectFromGitParametersValidator;
    }
}
