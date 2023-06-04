using BackendTools.Common.Models;
using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Common.Abstraction.Notification;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Chat.Team;
using Hackathon.IntegrationEvents.IntegrationEvent;
using MapsterMapper;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.BL.Chat;

public class TeamBaseChatService: BaseChatService<NewTeamChatMessage, TeamChatMessage>, ITeamChatService
{
    private readonly ITeamRepository _teamRepository;
    private readonly Common.Abstraction.IValidator<NewTeamChatMessage> _chatMessageValidator;

    public TeamBaseChatService(
        ITeamChatRepository teamChatRepository,
        ITeamRepository teamRepository,
        IMessageHub<ChatMessageChangedIntegrationEvent> chatMessageHub,
        IUserRepository userRepository,
        INotificationService notificationService,
        Common.Abstraction.IValidator<NewTeamChatMessage> chatMessageValidator,
        IMapper mapper):base(teamChatRepository, chatMessageHub, userRepository, notificationService, mapper)
    {
        _teamRepository = teamRepository;
        _chatMessageValidator = chatMessageValidator;
    }

    protected override Task<Result> ValidateAsync(NewTeamChatMessage message)
        => _chatMessageValidator.ValidateAsync(message);

    public new Task<Result> SendAsync(long ownerId, NewTeamChatMessage newTeamChatMessage)
        => base.SendAsync(ownerId, newTeamChatMessage);

    public Task<Result<BaseCollection<TeamChatMessage>>> GetListAsync(long teamId, int offset = 0, int limit = 300)
        => base.GetListAsync(teamId.ToString(), offset, limit);

    protected override void SetIntegrationEventUniqueParameter(ChatMessageChangedIntegrationEvent integrationEvent, NewTeamChatMessage newChatMessage)
    {
        integrationEvent.TeamId = newChatMessage.TeamId;
    }

    protected override Task EnrichMessageBeforeSaving<TChatMessageModel>(INewChatMessage newChatMessage, TChatMessageModel chatMessage)
    {
        if (newChatMessage is NewTeamChatMessage createTeamChatMessage && chatMessage is TeamChatMessage teamChatMessage)
        {
            teamChatMessage.TeamId = createTeamChatMessage.TeamId;
        }

        return Task.CompletedTask;
    }

    protected override async Task<long[]> GetUserIdsToNotify(long ownerId, NewTeamChatMessage newChatMessage)
    {
        var team = await _teamRepository.GetAsync(newChatMessage.TeamId);

        return team.Members
            .Where(x=> x.Id != ownerId)
            .Select(x => x.Id)
            .ToArray();
    }
}
