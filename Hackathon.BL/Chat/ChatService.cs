using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Common.Abstraction.Notification;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Notification;
using Hackathon.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvent;
using MapsterMapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.BL.Chat;

public abstract class ChatService<TNewChatMessage, TChatMessage>
    where TNewChatMessage: class, INewChatMessage
    where TChatMessage: class, IChatMessage
{
    private readonly IChatRepository<TChatMessage> _repository;

    private readonly IUserRepository _userRepository;
    private readonly IMessageHub<ChatMessageChangedIntegrationEvent> _chatMessageHub;
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    private readonly IValidator<INewChatMessage> _newMessageValidator;

    protected ChatService(
        IChatRepository<TChatMessage> repository,
        IMessageHub<ChatMessageChangedIntegrationEvent> chatMessageHub,
        IUserRepository userRepository,
        INotificationService notificationService,
        IValidator<INewChatMessage> newMessageValidator,
        IMapper mapper)
    {
        _repository = repository;
        _chatMessageHub = chatMessageHub;
        _userRepository = userRepository;
        _notificationService = notificationService;
        _newMessageValidator = newMessageValidator;
        _mapper = mapper;
    }

    protected async Task<Result<BaseCollection<TChatMessage>>> GetListAsync(string key, int offset = 0, int limit = 300)
    {
        var messages = await _repository.GetMessagesByKeyAsync(key, offset, limit);
        return Result<BaseCollection<TChatMessage>>.FromValue(messages);
    }

    protected async Task<Result> SendAsync(long ownerId, TNewChatMessage newChatMessage)
    {
        await _newMessageValidator.ValidateAndThrowAsync(newChatMessage);

        var typedChatMessage = await GetTypedChatMessage<TChatMessage>(ownerId, newChatMessage);

        typedChatMessage.Timestamp = DateTime.UtcNow;

        await _repository.AddMessageAsync(typedChatMessage);

        await PublicIntegrationEvent(newChatMessage);

        await NotifyUsersIfNeed(ownerId, newChatMessage);

        return Result.Success;
    }

    private async Task PublicIntegrationEvent(TNewChatMessage newChatMessage)
    {
        var integrationEvent = new ChatMessageChangedIntegrationEvent
        {
            Type = newChatMessage.Type
        };

        SetIntegrationEventUniqueParameter(integrationEvent, newChatMessage);

        await _chatMessageHub.Publish(TopicNames.ChatMessageChanged, integrationEvent);
    }

    /// <summary>
    /// Задать уникальный параметр сообщения интеграционного события
    /// </summary>
    /// <param name="integrationEvent">Сообщение интеграционного события</param>
    /// <param name="newChatMessage"></param>
    protected abstract void SetIntegrationEventUniqueParameter(ChatMessageChangedIntegrationEvent integrationEvent, TNewChatMessage newChatMessage);

    /// <summary>
    /// Получить список пользователей для которых необходимо отправить уведомление о новом сообщении
    /// </summary>
    /// <returns></returns>
    protected virtual Task<long[]> GetUserIdsToNotify(long ownerId, TNewChatMessage newChatMessage) => null;

    /// <summary>
    /// Обогатить сообщение дополнительными данными перед сохранением
    /// </summary>
    protected virtual Task EnrichMessageBeforeSaving<TChatMessageModel>(INewChatMessage newChatMessage, TChatMessageModel chatMessage)
    where TChatMessageModel: IChatMessage => Task.CompletedTask;

    /// <summary>
    /// Отправить уведомления пользователям, если это требуется
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="newChatMessage">Сообщение</param>
    private async Task NotifyUsersIfNeed(long ownerId, TNewChatMessage newChatMessage)
    {
        if (!newChatMessage.Options.HasFlag(ChatMessageOption.WithNotify))
            return;

        var userIdsToNotify = await GetUserIdsToNotify(ownerId, newChatMessage);

        if (userIdsToNotify is {Length: > 0})
        {
            var notificationModels = userIdsToNotify
                .Select(x =>
                    NotificationFactory.InfoNotification(newChatMessage.Message, x, ownerId));

            await _notificationService.PushManyAsync(notificationModels);
        }
    }

    private async Task<TChatMessageModel> GetTypedChatMessage<TChatMessageModel>(long ownerId, INewChatMessage newChatMessage)
    where TChatMessageModel: IChatMessage
    {
        var chatMessage = _mapper.Map<INewChatMessage, TChatMessageModel>(newChatMessage);

        if (chatMessage is null)
        {
            return default;
        }

        chatMessage.OwnerId = ownerId;

        var owner = await _userRepository.GetAsync(ownerId);
        chatMessage.OwnerFullName = owner.FullName;

        if (newChatMessage.UserId.HasValue)
        {
            var user = await _userRepository.GetAsync(newChatMessage.UserId.Value);
            chatMessage.UserFullName = user.FullName;
        }

        await EnrichMessageBeforeSaving(newChatMessage, chatMessage);

        return chatMessage;
    }
}
