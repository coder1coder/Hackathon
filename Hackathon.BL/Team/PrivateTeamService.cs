using BackendTools.Common.Models;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.ErrorMessages;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.BL.Team;

/// <summary>
/// Логика команд закрытого типа
/// </summary>
public class PrivateTeamService: IPrivateTeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITeamJoinRequestsRepository _teamJoinRequestsRepository;

    private const int SentJoinRequestsLimit = 5;

    public PrivateTeamService(ITeamRepository teamRepository,
        IUserRepository userRepository,
        ITeamJoinRequestsRepository teamJoinRequestsRepository)
    {
        _teamRepository = teamRepository;
        _userRepository = userRepository;
        _teamJoinRequestsRepository = teamJoinRequestsRepository;
    }

    public async Task<Result> CreateJoinRequestAsync(TeamJoinRequestCreateParameters parameters)
    {
        var user = await _userRepository.GetAsync(parameters.UserId);
        if (user is null)
            return Result.NotFound(UserMessages.UserDoesNotExists);

        var team = await _teamRepository.GetAsync(parameters.TeamId);
        if (team is null)
            return Result.NotFound(TeamMessages.TeamDoesNotExists);

        if (team.Type is not TeamType.Private)
            return Result.NotValid("Запрос на вступление в команду можно отправить только для команды закрытого типа");

        if (!team.OwnerId.HasValue)
            return Result.NotValid("Запрос на вступление в команду можно отправить только для пользовательских команд");

        if (team.HasMember(parameters.UserId))
            return Result.NotValid(TeamMessages.UserAlreadyIsTheTeamMember);

        if (team.IsFull())
            return Result.NotValid(TeamMessages.TeamIsFull);

        var sentJoinRequests = await _teamJoinRequestsRepository
            .GetListAsync(parameters.UserId, new Common.Models.GetListParameters<TeamJoinRequestFilter>
            {
                Filter = new TeamJoinRequestFilter
                {
                    Status = TeamJoinRequestStatus.Sent
                },
                Offset = 0,
                Limit = SentJoinRequestsLimit
            });

        if (sentJoinRequests?.Items?.Any(x=>x.TeamId == parameters.TeamId) == true)
            return Result.NotValid("Запрос на вступление в команду уже отправлен");

        if (sentJoinRequests?.Items is {Count: >= SentJoinRequestsLimit})
            return Result.NotValid("Превышено ограничение на количество запросов на вступление в команду");

        await _teamJoinRequestsRepository.CreateAsync(new TeamJoinRequestCreateParameters
        {
            TeamId = parameters.TeamId,
            UserId = parameters.UserId
        });

        return Result.Success;
    }

    public async Task<Result<TeamJoinRequestModel>> GetSentJoinRequestAsync(long teamId, long userId)
    {
        var sentRequests = await _teamJoinRequestsRepository.GetListAsync(userId, new Common.Models.GetListParameters<TeamJoinRequestFilter>
        {
            Filter = new TeamJoinRequestFilter
            {
                TeamId = teamId,
                Status = TeamJoinRequestStatus.Sent
            },
            Limit = 1
        });

        var firstSentRequest = sentRequests?.Items.FirstOrDefault();

        return firstSentRequest is null
            ? Result<TeamJoinRequestModel>.NotFound("Запрос на вступление в команду не найден")
            : Result<TeamJoinRequestModel>.FromValue(firstSentRequest);
    }

    public Task<BaseCollection<TeamJoinRequestModel>> GetJoinRequestsAsync(long userId, Common.Models.GetListParameters<TeamJoinRequestFilter> parameters)
        => _teamJoinRequestsRepository.GetListAsync(userId, parameters);

    public async Task<Result> CancelJoinRequestAsync(long teamId, long userId)
    {
        var joinRequests = await _teamJoinRequestsRepository
            .GetListAsync(userId, new Common.Models.GetListParameters<TeamJoinRequestFilter>
            {
                Filter = new TeamJoinRequestFilter
                {
                    TeamId = teamId,
                    Status = TeamJoinRequestStatus.Sent
                },
                Limit = 1
            });

        var joinRequest = joinRequests?.Items?.FirstOrDefault();

        if (joinRequest is null)
            return Result.NotFound("Запрос на вступление в команду не найден");

        await _teamJoinRequestsRepository.SetStatusAsync(joinRequest.Id, TeamJoinRequestStatus.Cancelled);
        return Result.Success;
    }
}
