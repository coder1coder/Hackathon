using FluentValidation;
using Hackathon.Common.Models.Event;

namespace Hackathon.BL.Validation.Event
{
    public class UpdateEventModelValidator: AbstractValidator<EventUpdateParameters>
    {
        public UpdateEventModelValidator(IValidator<BaseEventParameters> baseEventParametersValidator)
        {
            Include(baseEventParametersValidator);

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("необходимо указать корректный модификатор");
        }
    }
}
