using FluentValidation;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;

namespace Hackathon.BL.User.Validators
{
    public class SignInModelValidator: AbstractValidator<SignInModel>
    {
        public SignInModelValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100)
                .CustomAsync(async (userName, context, _) =>
                {
                    var users = await userRepository.GetAsync(new GetFilterModel<UserFilterModel>
                    {
                        Filter = new UserFilterModel
                        {
                            Username = userName
                        }
                    });

                    if (users.TotalCount == 0)
                        context.AddFailure("Пользователя с такими данными не существует");
                });

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(100);
        }
    }
}