using FluentValidation;
using Hackathon.Common.Abstraction;

namespace Hackathon.BL.User.Validators
{
    public class UserExistValidator: AbstractValidator<long>
    {
        public UserExistValidator(IUserRepository userRepository)
        {
            RuleFor(x => x)
                .MustAsync(async (userId, _) => await userRepository.ExistAsync(userId))
                .WithMessage("Пользователя с указанным идентификатором не существует");
        }
    }
}