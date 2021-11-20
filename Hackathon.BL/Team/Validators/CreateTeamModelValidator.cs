using FluentValidation;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Team;

namespace Hackathon.BL.Team.Validators
{
    public class CreateTeamModelValidator : AbstractValidator<CreateTeamModel>
    {
        public CreateTeamModelValidator(
            IEventRepository eventRepository,
            ITeamRepository teamRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30)
                .CustomAsync(async (teamName, context, _) =>
                {
                    var isSameNameExist = await teamRepository.ExistAsync(teamName);

                    if (isSameNameExist)
                        context.AddFailure("Команда с таким названием уже существует");
                });

            RuleFor(x => x.EventId)
                .GreaterThan(0)
                .CustomAsync(async (eventId, context, _) =>
                {
                    var isEventExist = await eventRepository.ExistAsync(eventId);

                    if (!isEventExist)
                        context.AddFailure("События с таким идентификатором не существует");

                    var eventModel = await eventRepository.GetAsync(eventId);

                    if (eventModel.Status != EventStatus.Published)
                        context.AddFailure("Зарегистрировать команду возможно только для опубликованного события");
                });


        }
    }
}