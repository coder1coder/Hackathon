using FluentValidation;
using Hackathon.Common.Models.Event;

namespace Hackathon.BL.Validation.Events;

public class CreateEventModelValidator : AbstractValidator<EventCreateParameters>
{
    public CreateEventModelValidator(IValidator<BaseEventParameters> baseEventParametersValidator)
    {
        Include(baseEventParametersValidator);

        RuleFor(x => x.ImageId)
            .NotEmpty()
            .WithMessage("Изображение обязательно к загрузке");
        
        RuleFor(x => x.OwnerId)
            .GreaterThan(0)
            .WithMessage("Идентификатор пользователя должен быть больше {ComparisonValue}");
    }
}
