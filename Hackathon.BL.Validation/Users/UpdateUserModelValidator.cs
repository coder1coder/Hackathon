using FluentValidation;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;

namespace Hackathon.BL.Validation.Users;

public class UpdateUserModelValidator: AbstractValidator<UpdateUserParameters>
{
    public UpdateUserModelValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Необходимо указать корректный идентификатор");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
    
        RuleFor(x => x)
           .CustomAsync(async (updateUserParameters, context, _) =>
           {
               var users = await userRepository.GetAsync(new GetListParameters<UserFilter>
               {
                   Filter = new UserFilter
                   {
                       Email = updateUserParameters.Email,
                       ExcludeIds = new [] { updateUserParameters.Id }
                   },
                   Limit = 1
               });

               if (users.Items.Count > 0)
                   context.AddFailure("Пользователь с таким Email уже зарегистрирован");
           });
    }
}

