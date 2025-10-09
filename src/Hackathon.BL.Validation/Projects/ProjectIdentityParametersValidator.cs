using FluentValidation;
using Hackathon.Common.Models.Projects;

namespace Hackathon.BL.Validation.Projects;

public class ProjectIdentityParametersValidator: AbstractValidator<IHasProjectIdentity>
{
    public ProjectIdentityParametersValidator()
    {
        RuleFor(x => x.TeamId)
            .GreaterThan(0)
            .WithMessage("Идентификатор команды должен быть больше 0");

        RuleFor(x => x.EventId)
            .GreaterThan(0)
            .WithMessage("Идентификатор события должен быть больше 0");
    }
}
