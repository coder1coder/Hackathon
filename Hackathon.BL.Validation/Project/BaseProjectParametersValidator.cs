﻿using FluentValidation;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Validation.Project;

public class BaseProjectParametersValidator: AbstractValidator<BaseProjectParameters>
{
    public BaseProjectParametersValidator(IValidator<IHasProjectIdentity> projectIdentityValidator)
    {
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .MaximumLength(200)
            .WithMessage("Название проекта должно содержать от {MinimumLength} до {MaximumLength} символов");

        Include(projectIdentityValidator);
    }
}
