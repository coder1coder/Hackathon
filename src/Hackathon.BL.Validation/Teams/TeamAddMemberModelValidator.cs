﻿using System.Linq;
using FluentValidation;
using Hackathon.BL.Validation.Users;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.Teams;

namespace Hackathon.BL.Validation.Teams;

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
                {
                    context.AddFailure("Команды с таким идентификатором не существует");
                }
            });

        RuleFor(x => x.MemberId)
            .GreaterThan(0)
            .WithMessage("Идентификатор пользователя должен быть больше 0")
            .CustomAsync(async (userId, context, _) =>
            {
                if (await userRepository.ExistsAsync(userId) == false)
                {
                    context.AddFailure(UserValidationErrorMessages.UserDoesNotExists);
                }
            });

        RuleFor(x => x)
            .CustomAsync(async (model, context, _) =>
            {
                var teamModel = await teamRepository.GetAsync(model.TeamId);

                if (teamModel is null)
                {
                    context.AddFailure(TeamValidationErrorMessages.TeamDoesNotExists);
                }

                if (teamModel is not null)
                {
                    if (teamModel.Members.Any(x => x.Id == model.MemberId))
                    {
                        context.AddFailure("Пользователь уже добавлен в эту команду");
                    }

                    if (teamModel.OwnerId == model.MemberId)
                    {
                        context.AddFailure("Нельзя добавить владельца команды в команду");
                    }
                }
            });

        RuleFor(x => x)
            .CustomAsync(async (model, context, _) =>
            {
                var teamMemberCount = await teamRepository.GetMembersCountAsync(model.TeamId);

                if (teamMemberCount >= TeamModel.MembersLimit)
                {
                    context.AddFailure(TeamValidationErrorMessages.MaximumNumberTeamMembersReached);
                }
            });
    }
}
