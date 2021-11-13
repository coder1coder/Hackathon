using FluentValidation;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Team;

namespace Hackathon.BL.Team.Validators
{
    public class CreateTeamModelValidator : AbstractValidator<CreateTeamModel>
    {
        public CreateTeamModelValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30);
        }
    }
}