using FluentValidation;
using Hackathon.Common.Models.Users;

namespace Hackathon.BL.Validation.Users;

public class SignInModelValidator: AbstractValidator<SignInModel>
{
    public SignInModelValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage(PasswordValidator.IncorrectPasswordError)
            .SetValidator(new PasswordValidator());
    }
}
