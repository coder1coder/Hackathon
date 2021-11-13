using FluentValidation;
using Hackathon.Common.Models.User;

namespace Hackathon.BL.User.Validators
{
    public class SignInModelValidator: AbstractValidator<SignInModel>
    {
        public SignInModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(100);
        }
    }
}