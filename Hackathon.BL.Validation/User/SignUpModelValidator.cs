using FluentValidation;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;

namespace Hackathon.BL.Validation.User;

public class SignUpModelValidator: AbstractValidator<SignUpModel>
{
    public SignUpModelValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .CustomAsync(async (userName, context, _) =>
            {
                var users = await userRepository.GetAsync(new GetListParameters<UserFilter>
                {
                    Filter = new UserFilter
                    {
                        Username = userName
                    }
                });

                if (users.TotalCount > 0)
                    context.AddFailure("Пользователь с таким логином уже зарегистрирован");
            });

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .EmailAddress()
            .CustomAsync(async (email, context, _) =>
            {
                var users = await userRepository.GetAsync(new GetListParameters<UserFilter>
                {
                    Filter = new UserFilter
                    {
                        Email = email
                    }
                });

                if (users.TotalCount > 0)
                    context.AddFailure("Пользователь с таким Email уже зарегистрирован");
            });

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
    }
}
