using FluentValidation;
using Hackathon.Common.Models.User;

namespace Hackathon.BL.Validation.Users;

public class UpdatePasswordModelValidator: AbstractValidator<UpdatePasswordModel>
{
    public UpdatePasswordModelValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x=>x.CurrentPassword)
            .NotNull()
            .WithMessage(PasswordValidator.IncorrectPasswordError)
            .SetValidator(new PasswordValidator());
        
        RuleFor(x => x.NewPassword)
            .NotNull()
            .WithMessage(PasswordValidator.IncorrectPasswordError)
            .SetValidator(new PasswordValidator());
    }
}
