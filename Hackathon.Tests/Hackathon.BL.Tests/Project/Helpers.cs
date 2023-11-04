using FluentValidation;
using Hackathon.BL.Validation.Project;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Tests.Project;

public static class Helpers
{
    public static IValidator<UpdateProjectFromGitBranchParameters> CreateValidator_UpdateProjectFromGitBranchParameters()
    {
        IValidator<IHasProjectIdentity> iHasProjectIdentityValidator = new ProjectIdentityParametersValidator();
        IValidator<UpdateProjectFromGitBranchParameters> updateProjectFromGitParametersValidator = 
            new UpdateProjectFromGitParametersValidator(iHasProjectIdentityValidator);

        return updateProjectFromGitParametersValidator;
    }
}
