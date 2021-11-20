using FluentValidation;
using Hackathon.Common.Abstraction;

namespace Hackathon.BL.Event.Validators
{
    public class EventExistValidator: AbstractValidator<long>
    {
        public EventExistValidator(IEventRepository eventRepository)
        {
            RuleFor(x => x)
                .MustAsync(async (eventId, _) => await eventRepository.ExistAsync(eventId))
                .WithMessage("События с указанным идентификатором не существует");
        }
    }
}