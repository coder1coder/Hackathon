using FluentValidation;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Validation.Project;

public class ProjectCreateParametersValidator: AbstractValidator<ProjectCreateParameters>
{
    public ProjectCreateParametersValidator(IValidator<BaseProjectParameters> baseParametersValidator)
    {
        Include(baseParametersValidator);
    }
}
