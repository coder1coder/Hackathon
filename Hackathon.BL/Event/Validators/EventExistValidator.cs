using FluentValidation;
using Hackathon.Common.Abstraction;

namespace Hackathon.BL.Event.Validators
{
    public class EventExistValidator: AbstractValidator<long[]>
    {
        public EventExistValidator(IEventRepository eventRepository)
        {
            RuleFor(x => x.Length)
                .GreaterThan(0)
                .WithMessage("Невозможно проверить существование пустого списка событий");

            RuleFor(x => x)
                .MustAsync(async (eventsIds, _) => await eventRepository.ExistAsync(eventsIds))
                .WithMessage(eventIds=>
                    eventIds.Length == 1
                    ? $"События с идентификатором {eventIds} не существует"
                    : "Не все из указанных событий существуют"
                    );
        }
    }
}