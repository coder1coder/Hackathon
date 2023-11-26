using System.Linq;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.BL.Validation.User;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.ErrorMessages;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using Hackathon.Informing.Abstractions.Models.Notifications.Data;
using Hackathon.Informing.Abstractions.Services;
using Hackathon.Informing.BL;

namespace Hackathon.BL.Team;

/// <summary>
/// Логика команд закрытого типа
/// </summary>
public class PrivateTeamService: IPrivateTeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITeamJoinRequestsRepository _teamJoinRequestsRepository;
    private readonly INotificationService _notificationService;

    private const string TeamJoinRequestNotFound = "Запрос на вступление в команду не найден";
    private const int SentJoinRequestsLimit = 5;

    public PrivateTeamService(
        ITeamRepository teamRepository,
        IUserRepository userRepository,
        ITeamJoinRequestsRepository teamJoinRequestsRepository,
        INotificationService notificationService)
    {
        _teamRepository = teamRepository;
        _userRepository = userRepository;
        _teamJoinRequestsRepository = teamJoinRequestsRepository;
        _notificationService = notificationService;
    }

    public async Task<Result<long>> CreateJoinRequestAsync(TeamJoinRequestCreateParameters parameters)
    {
        var user = await _userRepository.GetAsync(parameters.UserId);
        if (user is null)
            return Result<long>.NotFound(UserMessages.UserDoesNotExists);

        var team = await _teamRepository.GetAsync(parameters.TeamId);
        if (team is null)
            return Result<long>.NotFound(TeamMessages.TeamDoesNotExists);

        if (team.Type is not TeamType.Private)
            return Result<long>.NotValid("Запрос на вступление в команду можно отправить только для команды закрытого типа");

        if (!team.OwnerId.HasValue)
            return Result<long>.NotValid("Запрос на вступление в команду можно отправить только для пользовательских команд");

        if (team.HasMemberWithId(parameters.UserId))
            return Result<long>.NotValid(TeamMessages.UserAlreadyIsTheTeamMember);

        if (team.IsFull())
            return Result<long>.NotValid(TeamMessages.TeamIsFull);

        var sentJoinRequests = await _teamJoinRequestsRepository
            .GetListAsync(new Common.Models.GetListParameters<TeamJoinRequestExtendedFilter>
            {
                Filter = new TeamJoinRequestExtendedFilter
                {
                    UserId = parameters.UserId,
                    Status = TeamJoinRequestStatus.Sent
                },
                Offset = 0,
                Limit = SentJoinRequestsLimit
            });

        if (sentJoinRequests?.Items?.Any(x=>x.TeamId == parameters.TeamId) == true)
            return Result<long>.NotValid("Запрос на вступление в команду уже отправлен");

        if (sentJoinRequests?.Items is {Count: >= SentJoinRequestsLimit})
            return Result<long>.NotValid("Превышено ограничение на количество запросов на вступление в команду");

        var requestId = await _teamJoinRequestsRepository.CreateAsync(new TeamJoinRequestCreateParameters
        {
            TeamId = parameters.TeamId,
            UserId = parameters.UserId
        });

        return Result<long>.FromValue(requestId);
    }

    public async Task<Result<TeamJoinRequestModel>> GetSingleSentJoinRequestAsync(long teamId, long userId)
    {
        var sentRequests = await _teamJoinRequestsRepository.GetListAsync(new Common.Models.GetListParameters<TeamJoinRequestExtendedFilter>
        {
            Filter = new TeamJoinRequestExtendedFilter
            {
                UserId = userId,
                TeamId = teamId,
                Status = TeamJoinRequestStatus.Sent
            },
            Limit = 1
        });

        var firstSentRequest = sentRequests?.Items.FirstOrDefault();

        return firstSentRequest is null
            ? Result<TeamJoinRequestModel>.NotFound(TeamJoinRequestNotFound)
            : Result<TeamJoinRequestModel>.FromValue(firstSentRequest);
    }

    public Task<BaseCollection<TeamJoinRequestModel>> GetJoinRequestsAsync(Common.Models.GetListParameters<TeamJoinRequestExtendedFilter> parameters)
        => _teamJoinRequestsRepository.GetListAsync(parameters);

    public async Task<Result<BaseCollection<TeamJoinRequestModel>>> GetTeamSentJoinRequests(long teamId, long authorizedUserId, PaginationSort paginationSort)
    {
        var team = await _teamRepository.GetAsync(teamId);
        if (team is null)
            return Result<BaseCollection<TeamJoinRequestModel>>.NotFound(TeamMessages.TeamDoesNotExists);

        if (team.OwnerId != authorizedUserId)
            return Result<BaseCollection<TeamJoinRequestModel>>.Forbidden(
                "Получение списка запросов на вступление в команду доступно только уполномоченным лицам");

        var result = await _teamJoinRequestsRepository.GetListAsync(new Common.Models.GetListParameters<TeamJoinRequestExtendedFilter>
        {
            Filter = new TeamJoinRequestExtendedFilter
            {
                TeamId = teamId,
                Status = TeamJoinRequestStatus.Sent
            },
            Limit = paginationSort.Limit,
            Offset = paginationSort.Offset,
            SortBy = paginationSort.SortBy,
            SortOrder = paginationSort.SortOrder
        });

        return Result<BaseCollection<TeamJoinRequestModel>>.FromValue(result);
    }

    public async Task<Result> ApproveJoinRequest(long authorizedUserId, long requestId)
    {
        var request = await _teamJoinRequestsRepository.GetAsync(requestId);

        if (request is null)
            return Result.NotFound(TeamJoinRequestNotFound);

        var isTeamOwner = request.TeamOwnerId == authorizedUserId;

        if (!isTeamOwner)
            return Result.Forbidden("Принять запрос на вступление в команду может только владелец команды или уполномоченное лицо");

        var user = await _userRepository.GetAsync(request.UserId);

        if (user is null)
            return Result.NotValid(UserErrorMessages.UserDoesNotExists);

        var team = await _teamRepository.GetAsync(request.TeamId);
        if (team is null)
            return Result.NotFound(TeamMessages.TeamDoesNotExists);

        if (team.HasMemberWithId(request.UserId))
            return Result<long>.NotValid(TeamMessages.UserAlreadyIsTheTeamMember);

        await _teamJoinRequestsRepository.SetStatusWithCommentAsync(requestId, TeamJoinRequestStatus.Accepted);

        await _teamRepository.AddMemberAsync(new TeamMemberModel
        {
            TeamId = request.TeamId,
            MemberId = request.UserId,
            Role = TeamRole.Participant
        });

        await _notificationService.PushAsync(NotificationCreator.TeamJoinRequestDecision(new TeamJoinRequestDecisionData
            {
                TeamId = team.Id,
                TeamName = team.Name,
                Comment = null,
                IsApproved = true
            }, request.UserId, authorizedUserId));

        return Result.Success;
    }

    public async Task<Result> CancelJoinRequestAsync(long authorizedUserId, CancelRequestParameters parameters)
    {
        var request = await _teamJoinRequestsRepository.GetAsync(parameters.RequestId);

        if (request is null)
            return Result.NotFound(TeamJoinRequestNotFound);

        var isRequestAuthor = authorizedUserId == request.UserId;
        var isTeamOwner = authorizedUserId == request.TeamOwnerId;

        if (!isRequestAuthor && !isTeamOwner)
            return Result.Forbidden("Отменить запрос на вступление в команду может только автор запроса или уполномоченный участник команды");

        if (request.Status is not TeamJoinRequestStatus.Sent)
            return Result.NotValid("Запрос уже принят или отклонен");
        
        var team = await _teamRepository.GetAsync(request.TeamId);
        if (team is null)
            return Result.NotFound(TeamMessages.TeamDoesNotExists);

        var newStatus = isTeamOwner ? TeamJoinRequestStatus.Refused : TeamJoinRequestStatus.Cancelled;

        await _teamJoinRequestsRepository.SetStatusWithCommentAsync(parameters.RequestId, newStatus, parameters.Comment);

        if (isTeamOwner)
        {
            await _notificationService.PushAsync(NotificationCreator.TeamJoinRequestDecision(new TeamJoinRequestDecisionData
                {
                    TeamId = team.Id,
                    TeamName = team.Name,
                    Comment = parameters.Comment,
                    IsApproved = false
                }, request.UserId, authorizedUserId));
        }

        return Result.Success;
    }
}
