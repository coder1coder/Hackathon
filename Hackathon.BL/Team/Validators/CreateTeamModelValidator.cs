using FluentValidation;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Team;

namespace Hackathon.BL.Team.Validators
{
    public class CreateTeamModelValidator : AbstractValidator<CreateTeamModel>
    {
        public CreateTeamModelValidator(ITeamRepository teamRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30)
                .CustomAsync(async (teamName, context, _) =>
                {
                    var isSameNameExist = await teamRepository.ExistAsync(teamName);

                    if (isSameNameExist)
                        context.AddFailure("Команда с таким названием уже существует");
                });
        }
    }
}