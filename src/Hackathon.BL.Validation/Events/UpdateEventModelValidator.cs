using FluentValidation;
using Hackathon.Common.Models.Event;

namespace Hackathon.BL.Validation.Events;

public class UpdateEventModelValidator: AbstractValidator<EventUpdateParameters>
{
    public UpdateEventModelValidator(IValidator<BaseEventParameters> baseEventParametersValidator)
    {
        Include(baseEventParametersValidator);
        
        RuleFor(x => x.ImageId)
            .NotEmpty()
            .WithMessage("Изображение обязательно к загрузке");
        
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("необходимо указать корректный идентификатор");

        When(x => x.Stages is {Count: > 0}, () =>
        {
            RuleForEach(x => x.Stages).ChildRules(v =>
                v.RuleFor(x => x.EventId)
                    .GreaterThan(0));
        });
    }
}
