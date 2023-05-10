using FluentValidation;
using Hackathon.Common.Models.Chat;

namespace Hackathon.BL.Validation.Chat;

public class NewChatMessageValidator: AbstractValidator<INewChatMessage>
{
    public NewChatMessageValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("{Property} обязательно к заполнению")
            .MinimumLength(1)
            .MaximumLength(200)
            .WithMessage("{PropertyName} допускает длину от {MinLength} до {MaxLength} символов")
            .WithName("Сообщение");

        RuleFor(x => x.Options)
            .IsInEnum();
    }
}
