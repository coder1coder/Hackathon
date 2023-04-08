using FluentValidation;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Validation.Project;

public class ProjectUpdateParametersValidator: AbstractValidator<ProjectUpdateParameters>
{
    public ProjectUpdateParametersValidator(IValidator<BaseProjectParameters> baseParametersValidator)
    {
        Include(baseParametersValidator);
    }
}
