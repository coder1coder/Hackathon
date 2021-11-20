using FluentValidation;
using Hackathon.Common.Abstraction;

namespace Hackathon.BL.Team.Validators
{
    public class TeamExistValidator: AbstractValidator<long>
    {
        public TeamExistValidator(ITeamRepository teamRepository)
        {
            RuleFor(x => x)
                .MustAsync(async (teamId, _) => await teamRepository.ExistAsync(teamId));
        }
    }
}