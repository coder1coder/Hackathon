using FluentValidation;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Validation.Project;

public class ProjectIdentityParametersValidator: AbstractValidator<IHasProjectIdentity>
{
    public ProjectIdentityParametersValidator()
    {
        RuleFor(x => x.TeamId)
            .GreaterThan(default(long))
            .WithMessage("Идентификатор команды должен быть больше 0");

        RuleFor(x => x.EventId)
            .GreaterThan(default(long))
            .WithMessage("Идентификатор события должен быть больше 0");
    }
}
