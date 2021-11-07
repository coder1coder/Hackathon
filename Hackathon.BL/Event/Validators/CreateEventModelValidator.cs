using FluentValidation;
using Hackathon.Common.Models;

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
        }
    }
}