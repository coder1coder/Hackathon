using FluentValidation;
using Hackathon.Common.Models;
using System;

namespace Hackathon.BL.Event.Validators
{
    public class CreateEventModelValidator: AbstractValidator<CreateEventModel>
    {
        public CreateEventModelValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(100); 
            
            RuleFor(x => x.Start)
               .NotNull()
               .NotEmpty()
               .GreaterThan(DateTime.UtcNow).WithMessage("Значение должно быть больше текущих даты и времени");
        }
    }
}