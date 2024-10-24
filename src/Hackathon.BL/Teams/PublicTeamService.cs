using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.BL.Validation.Users;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.Teams;

namespace Hackathon.BL.Teams;

public class PublicTeamService: IPublicTeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;

    public PublicTeamService(ITeamRepository teamRepository, IUserRepository userRepository)
    {
        _teamRepository = teamRepository;
        _userRepository = userRepository;
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

        return Result.Success;
    }
}
