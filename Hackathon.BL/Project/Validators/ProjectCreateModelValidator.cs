using FluentValidation;
using Hackathon.Abstraction.Team;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Project.Validators
{
    public class ProjectCreateModelValidator: AbstractValidator<ProjectCreateModel>
    {
        public ProjectCreateModelValidator(ITeamRepository teamRepository)
        {
            RuleFor(x => x.Name)
                .MinimumLength(3)
                .MaximumLength(200)
                .WithMessage("Название проекта должно содержать от {MinimumLength} до {MaximumLength} символов");

            RuleFor(x => x.TeamId)
                .GreaterThan(default(long))
                .WithMessage("Идентификатор команды должен быть больше 0");
        }
    }
}