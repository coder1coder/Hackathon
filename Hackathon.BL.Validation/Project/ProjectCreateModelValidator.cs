using FluentValidation;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Validation.Project
{
    public class ProjectCreateModelValidator: AbstractValidator<ProjectCreateParameters>
    {
        public ProjectCreateModelValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(3)
                .MaximumLength(200)
                .WithMessage("Название проекта должно содержать от {MinimumLength} до {MaximumLength} символов");

            RuleFor(x => x.TeamId)
                .GreaterThan(default(long))
                .WithMessage("Идентификатор команды должен быть больше 0");

            RuleFor(x => x.EventId)
                .GreaterThan(default(long))
                .WithMessage("Идентификатор события должен быть больше 0");
        }
    }
}
