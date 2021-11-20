using FluentValidation;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Event;
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

            RuleFor(x=>x.TeamId)
                .CustomAsync(async (teamId, context, _) => {

                    var team = await teamRepository.GetAsync(teamId);

                    if (team == null)
                    {
                        context.AddFailure("Команды с указаным идентификатором не существует");
                    }
                    else
                    {
                        if (team.Project != null)
                            context.AddFailure("Команда уже имеет проект");

                        if (team.Event.Status != EventStatus.Published)
                            context.AddFailure("Невозможно добавить проект пока событие не опубликовано");
                    }
                });
        }
    }
}