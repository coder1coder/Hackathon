﻿using System;
using System.Linq;
using FluentValidation;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Users;
using Hackathon.Configuration;
using Microsoft.Extensions.Options;

namespace Hackathon.BL.Validation.Users;

public class SignUpModelValidator: AbstractValidator<CreateNewUserModel>
{
    public const string SpecifyUserNameIsRestricted = "Указанное имя пользователя не допустимо";

    public SignUpModelValidator(IUserRepository userRepository, IOptions<RestrictedNames> restrictedNamesOptions)
    {
        var restrictedNames = restrictedNamesOptions?.Value?.Users ?? Array.Empty<string>();

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .CustomAsync(async (userName, context, _) =>
            {
                if (restrictedNames.Contains(userName.Trim().ToLower()))
                {
                    context.AddFailure(SpecifyUserNameIsRestricted);
                    return;
                }

                var users = await userRepository.GetAsync(new GetListParameters<UserFilter>
                {
                    Filter = new UserFilter
                    {
                        Username = userName
                    }
                });

                if (users.TotalCount > 0)
                {
                    context.AddFailure("Пользователь с таким логином уже зарегистрирован");
                }
            });

        When(x => x.GoogleAccount is null, () =>
        {
            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage(PasswordValidator.IncorrectPasswordError)
                .SetValidator(new PasswordValidator());
        });

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
                {
                    context.AddFailure("Пользователь с таким Email уже зарегистрирован");
                }
            });

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
    }
}
