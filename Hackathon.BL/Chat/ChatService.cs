using BackendTools.Common.Models;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Abstraction.Chat;
using Hackathon.Abstraction.IntegrationEvents;
using Hackathon.Abstraction.Notification;
using Hackathon.Abstraction.Team;
using Hackathon.Abstraction.User;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Notification;
using Hackathon.Entities;
using Hackathon.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvent;
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

    private readonly IValidator<ICreateChatMessage> _chatMessageValidator;

    public ChatService(
        IChatRepository chatRepository,
        IMessageHub<ChatMessageChangedIntegrationEvent> chatMessageHub,
        IUserRepository userRepository,
        INotificationService notificationService,
        ITeamRepository teamRepository,
        ILogger<ChatService> logger,
        IValidator<ICreateChatMessage> chatMessageValidator)
    {
        _chatRepository = chatRepository;
        _chatMessageHub = chatMessageHub;
        _userRepository = userRepository;
        _notificationService = notificationService;
        _teamRepository = teamRepository;
        _logger = logger;
        _chatMessageValidator = chatMessageValidator;
    }

    public async Task<Result> SendMessage(ICreateChatMessage createChatMessage)
    {
        await _chatMessageValidator.ValidateAndThrowAsync(createChatMessage);

        var entity = new ChatMessageEntity
        {
            Type = createChatMessage.Type,
            Message = createChatMessage.Message,
            Timestamp = createChatMessage.Timestamp,
            Options = createChatMessage.Options,
            OwnerId = createChatMessage.OwnerId,
            UserId = createChatMessage.UserId,
            TeamId = createChatMessage is CreateTeamChatMessage createTeamChatMessage ? createTeamChatMessage.TeamId : null
        };

        await EnrichChatMessage(entity);

        await _chatRepository.AddMessage(entity);
        await _chatMessageHub.Publish(TopicNames.ChatMessageChanged, new ChatMessageChangedIntegrationEvent
        {
            Type = createChatMessage.Type,
            TeamId = entity.TeamId
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

            await _notificationService.PushMany(notificationModels);
        }
    }

    private async Task EnrichChatMessage(ChatMessageEntity chatMessageEntity)
    {
        var owner = await _userRepository.GetAsync(chatMessageEntity.OwnerId);
        chatMessageEntity.OwnerFullName = owner.FullName;

        if (chatMessageEntity.UserId.HasValue)
        {
            var user = await _userRepository.GetAsync(chatMessageEntity.UserId.Value);
            chatMessageEntity.UserFullName = user.FullName;
        }
    }
}
