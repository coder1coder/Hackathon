using System;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Chats.Abstractions.IntegrationEvents;
using Hackathon.Chats.Abstractions.Models;
using Hackathon.Chats.Abstractions.Models.Teams;
using Hackathon.Chats.Abstractions.Repositories;
using Hackathon.Chats.Abstractions.Services;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Informing.Abstractions.Services;
using MapsterMapper;

namespace Hackathon.Chats.BL.Services;

public class TeamChatService: BaseChatService<NewTeamChatMessage, TeamChatMessage>, ITeamChatService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IValidator<NewTeamChatMessage> _chatMessageValidator;
    private readonly ITeamChatHub _hub;

    public TeamChatService(
        ITeamChatRepository teamChatRepository,
        ITeamRepository teamRepository,
        IUserRepository userRepository,
        INotificationService notificationService,
        IValidator<NewTeamChatMessage> chatMessageValidator,
        IMapper mapper, 
        ITeamChatHub hub) : base(teamChatRepository, userRepository, notificationService, mapper)
    {
        _teamRepository = teamRepository;
        _chatMessageValidator = chatMessageValidator;
        _hub = hub;
    }

    protected override Task<Result> ValidateNewMessageAsync(NewTeamChatMessage message)
        => _chatMessageValidator.ValidateAsync(message);

    public new Task<Result> SendMessageAsync(long ownerId, NewTeamChatMessage newTeamChatMessage)
    {
        return base.SendMessageAsync(ownerId, newTeamChatMessage);
    }

    protected override Task PublicIntegrationEvent(Guid messageId, NewTeamChatMessage newMessage)
    {
        return _hub.SendEventAsync(new TeamChatNewMessageIntegrationEvent(newMessage.TeamId, messageId));
    }

    protected override Task EnrichMessageBeforeSaving<TChatMessageModel>(INewChatMessage newChatMessage, TChatMessageModel chatMessage)
    {
        if (newChatMessage is NewTeamChatMessage createTeamChatMessage && chatMessage is TeamChatMessage teamChatMessage)
        {
            teamChatMessage.TeamId = createTeamChatMessage.TeamId;
        }

        return Task.CompletedTask;
    }

    protected override Task<long[]> GetUserIdsToNotify(long ownerId, NewTeamChatMessage newChatMessage)
    {
        return _teamRepository.GetTeamMemberIdsAsync(newChatMessage.TeamId, ownerId);
    }
}
