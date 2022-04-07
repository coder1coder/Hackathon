using FluentValidation;
using Hackathon.Abstraction;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.ProjectMember;

namespace Hackathon.BL.ProjectMember.Validators;

public class CreateProjectMemberModelValidator : AbstractValidator<CreateProjectMemberModel>
{
    public CreateProjectMemberModelValidator(IProjectMemberRepository projectMemberRepository)
    {
        RuleFor(x => x.UserId)
            .GreaterThan(default(long))
            .WithMessage("Идентификатор пользователя должен быть больше 0");

        RuleFor(x => x.TeamId)
            .GreaterThan(default(long))
            .WithMessage("Идентификатор команды должен быть больше 0");

        RuleFor(x => x.ProjectId)
            .GreaterThan(default(long))
            .WithMessage("Идентификатор проекта должен быть больше 0");

        RuleFor(x => x.EventId)
            .GreaterThan(default(long))
            .WithMessage("Идентификатор события должен быть больше 0");
    }
}