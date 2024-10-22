using FluentValidation;

namespace Hackathon.BL.Validation.Users;

public class PasswordValidator: AbstractValidator<string>
{
    public const string IncorrectPasswordError = "Некорректный пароль";
    
    public PasswordValidator()
    {
        RuleFor(x => x)
            .Must(x => x is { Length: >= 6, Length: <= 20 })
            .WithMessage(IncorrectPasswordError);
    }
}
