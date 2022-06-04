using System.Linq;
using FluentValidation;
using Hackathon.Abstraction.Team;
using Hackathon.Abstraction.User;
using Hackathon.Common.Models.Team;

namespace Hackathon.BL.Team.Validators
{
    public class TeamAddMemberModelValidator: AbstractValidator<TeamMemberModel>
    {
        public TeamAddMemberModelValidator(
            ITeamRepository teamRepository,
            IUserRepository userRepository
            )
        {
            RuleFor(x => x.TeamId)
                .GreaterThan(0)
                .WithMessage("Идентификатор команды должен быть больше 0")
                .CustomAsync(async (teamId, context, _) =>
                {
                    if (await teamRepository.ExistAsync(teamId) == false)
                        context.AddFailure("Команды с таким идентификатором не существует");
                });

            RuleFor(x => x.MemberId)
                .GreaterThan(0)
                .WithMessage("Идентификатор пользователя должен быть больше 0")
                .CustomAsync(async (userId, context, _) =>
                {
                    if (await userRepository.ExistAsync(userId) == false)
                        context.AddFailure("Пользователя с таким идентификатором не существует");
                });

            RuleFor(x => x)
                .CustomAsync(async (model, context, _) =>
                {
                    var teamModel = await teamRepository.GetAsync(model.TeamId);

                    if (teamModel.Members.Any(x => x.Id == model.MemberId))
                        context.AddFailure("Пользователь уже добавлен в эту команду");
                });

            RuleFor(x => x)
                .CustomAsync(async (model, context, _) =>
                {
                    var teamMemberCount = await teamRepository.GetMembersCount(model.TeamId);

                    if (teamMemberCount >= TeamService.MAX_TEAM_MEMBERS)
                        context.AddFailure($"Достигнуто максимальное количество участников в команде {teamMemberCount} из {TeamService.MAX_TEAM_MEMBERS}");
                });
        }
    }
}