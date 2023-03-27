using BackendTools.Common.Models;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Common.Abstraction.Notification;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Chat.Team;
using Hackathon.Common.Models.Notification;
using Hackathon.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvent;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace Hackathon.BL.Chat;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IMessageHub<ChatMessageChangedIntegrationEvent> _chatMessageHub;
    private readonly INotificationService _notificationService;
    private readonly ILogger<ChatService> _logger;
    private readonly IMapper _mapper;

    private readonly IValidator<ICreateChatMessage> _chatMessageValidator;

    public ChatService(
        IChatRepository chatRepository,
        IMessageHub<ChatMessageChangedIntegrationEvent> chatMessageHub,
        IUserRepository userRepository,
        INotificationService notificationService,
        ITeamRepository teamRepository,
        ILogger<ChatService> logger,
        IValidator<ICreateChatMessage> chatMessageValidator,
        IMapper mapper)
    {
        _chatRepository = chatRepository;
        _chatMessageHub = chatMessageHub;
        _userRepository = userRepository;
        _notificationService = notificationService;
        _teamRepository = teamRepository;
        _logger = logger;
        _chatMessageValidator = chatMessageValidator;
        _mapper = mapper;
    }

    public async Task<Result> SendMessage<TChatMessageModel>(long ownerId, ICreateChatMessage createChatMessage)
        where TChatMessageModel: IChatMessage
    {
        createChatMessage.OwnerId = ownerId;

        await _chatMessageValidator.ValidateAndThrowAsync(createChatMessage);

        var typedChatMessage = await GetTypedChatMessage<TChatMessageModel>(createChatMessage);

        await _chatRepository.AddMessage(typedChatMessage);

        await _chatMessageHub.Publish(TopicNames.ChatMessageChanged, new ChatMessageChangedIntegrationEvent
        {
            Type = createChatMessage.Type,
            TeamId = typedChatMessage is TeamChatMessage teamChatMessage ? teamChatMessage.TeamId : null
        });

        await NotifyUsersAboutNewMessageIfNeed(createChatMessage);

        return Result.Success;
    }

    public async Task<Result<BaseCollection<TeamChatMessage>>> GetTeamMessages(long teamId, int offset = 0, int limit = 300)
    {
        var messages = await _chatRepository.GetTeamChatMessages(teamId, offset, limit);
        return Result<BaseCollection<TeamChatMessage>>.FromValue(messages);
    }

    /// <summary>
    /// Отправить уведомления пользователям, если это требуется
    /// </summary>
    /// <param name="createChatMessage">Сообщение</param>
    private async Task NotifyUsersAboutNewMessageIfNeed(ICreateChatMessage createChatMessage)
    {
        if (!createChatMessage.Options.HasFlag(ChatMessageOption.WithNotify))
            return;

        switch (createChatMessage.Type)
        {
            case ChatMessageType.TeamChat when createChatMessage is CreateTeamChatMessage teamChatMessage:
                await NotifyTeamAboutNewMessage(teamChatMessage);
                break;

            default:
                _logger.LogWarning("{Service}.{Action} doesn't have any handler message for type {messageType}",
                    nameof(ChatService),
                    nameof(NotifyUsersAboutNewMessageIfNeed),
                    createChatMessage.Type);
                break;
        }
    }

    private async Task NotifyTeamAboutNewMessage(CreateTeamChatMessage chatMessage)
    {
        var team = await _teamRepository.GetAsync(chatMessage.TeamId);

        var usersIds = team.Members
            .Where(x=> x.Id != chatMessage.OwnerId)
            .Select(x => x.Id)
            .ToArray();

        if (usersIds.Length > 0)
        {
            var notificationModels = usersIds
                .Select(x =>
                    NotificationFactory.InfoNotification(chatMessage.Message, x, chatMessage.OwnerId));

            await _notificationService.PushManyAsync(notificationModels);
        }
    }

    private async Task<TChatMessageModel> GetTypedChatMessage<TChatMessageModel>(ICreateChatMessage createChatMessage)
    where TChatMessageModel: IChatMessage
    {
        var chatMessage = _mapper.Map<ICreateChatMessage, TChatMessageModel>(createChatMessage);

        if (chatMessage is null)
        {
            return default;
        }

        var owner = await _userRepository.GetAsync(createChatMessage.OwnerId);
        chatMessage.OwnerFullName = owner.FullName;

        if (createChatMessage.UserId.HasValue)
        {
            var user = await _userRepository.GetAsync(createChatMessage.UserId.Value);
            chatMessage.UserFullName = user.FullName;
        }

        if (createChatMessage is CreateTeamChatMessage createTeamChatMessage && chatMessage is TeamChatMessage teamChatMessage)
        {
            teamChatMessage.TeamId = createTeamChatMessage.TeamId;
        }

        return chatMessage;
    }
}
