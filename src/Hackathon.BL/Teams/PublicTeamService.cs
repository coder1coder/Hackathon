using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.BL.Validation.Users;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Messages.Teams;
using Hackathon.Common.Models.Teams;
using Hackathon.Infrastructure;

namespace Hackathon.BL.Teams;

public class PublicTeamService: IPublicTeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMessageBusService _messageBusService;

    public PublicTeamService(
        ITeamRepository teamRepository, 
        IUserRepository userRepository, 
        IMessageBusService messageBusService)
    {
        _teamRepository = teamRepository;
        _userRepository = userRepository;
        _messageBusService = messageBusService;
    }

    public async Task<Result> JoinToTeamAsync(long teamId, long authorizedUserId)
    {
        var team = await _teamRepository.GetAsync(teamId);

        if (team is null)
        {
            return Result.NotValid(TeamErrorMessages.TeamDoesNotExists);
        }

        if (team.Type != TeamType.Public)
        {
            return Result.NotValid(TeamErrorMessages.SelectedTeamIsNotPublic);
        }

        var user = await _userRepository.GetAsync(authorizedUserId);

        if (user is null)
        {
            return Result.NotValid(UserValidationErrorMessages.UserDoesNotExists);
        }

        if (team.HasMemberWithId(authorizedUserId))
        {
            return Result.NotValid(TeamErrorMessages.UserAlreadyIsTheTeamMember);
        }

        await _teamRepository.AddMemberAsync(new TeamMemberModel
        {
            TeamId = teamId,
            MemberId = authorizedUserId,
            Role = TeamRole.Participant
        });

        await _messageBusService.TryPublishAsync(new NewTeamMemberMessage(teamId, authorizedUserId));

        return Result.Success;
    }
}
