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

    public TeamChatService(
        ITeamChatRepository teamChatRepository,
        ITeamRepository teamRepository,
        IChatsIntegrationEventsHub integrationEventsHub,
        IUserRepository userRepository,
        INotificationService notificationService,
        IValidator<NewTeamChatMessage> chatMessageValidator,
        IMapper mapper):base(teamChatRepository, integrationEventsHub, userRepository, notificationService, mapper)
    {
        _teamRepository = teamRepository;
        _chatMessageValidator = chatMessageValidator;
    }

    protected override Task<Result> ValidateAsync(NewTeamChatMessage message)
        => _chatMessageValidator.ValidateAsync(message);

    public new Task<Result> SendAsync(long ownerId, NewTeamChatMessage newTeamChatMessage)
        => base.SendAsync(ownerId, newTeamChatMessage);

    protected override Task PublicIntegrationEvent(Guid messageId, NewTeamChatMessage newMessage)
        => IntegrationEventsHub.PublishAll(new TeamChatNewMessageIntegrationEvent
        {
            TeamId = newMessage.TeamId,
            MessageId = messageId
        });

    protected override Task EnrichMessageBeforeSaving<TChatMessageModel>(INewChatMessage newChatMessage, TChatMessageModel chatMessage)
    {
        if (newChatMessage is NewTeamChatMessage createTeamChatMessage && chatMessage is TeamChatMessage teamChatMessage)
        {
            teamChatMessage.TeamId = createTeamChatMessage.TeamId;
        }

        return Task.CompletedTask;
    }

    protected override Task<long[]> GetUserIdsToNotify(long ownerId, NewTeamChatMessage newChatMessage)
        => _teamRepository.GetTeamMemberIdsAsync(newChatMessage.TeamId, ownerId);
}
