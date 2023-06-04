using FluentValidation;
using Hackathon.Common.Models.Event;

namespace Hackathon.BL.Validation.Event;

public class CreateEventModelValidator : AbstractValidator<EventCreateParameters>
{
    public CreateEventModelValidator(IValidator<BaseEventParameters> baseEventParametersValidator)
    {
        Include(baseEventParametersValidator);

        RuleFor(x => x.OwnerId)
            .GreaterThan(0)
            .WithMessage("Идентификатор пользователя должен быть больше {ComparisonValue}");
    }
}
