using System;
using System.Linq;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Chats.Abstractions.IntegrationEvents;
using Hackathon.Chats.Abstractions.Models;
using Hackathon.Chats.Abstractions.Repositories;
using Hackathon.Chats.Abstractions.Services;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.Base;
using Hackathon.Informing.Abstractions.Models;
using Hackathon.Informing.Abstractions.Services;
using MapsterMapper;

namespace Hackathon.Chats.BL.Services;

public abstract class BaseChatService<TNewChatMessage, TChatMessage>: IChatService<TNewChatMessage, TChatMessage>
    where TNewChatMessage: class, INewChatMessage
    where TChatMessage: class, IChatMessage
{
    private readonly IChatRepository<TChatMessage> _repository;

    private readonly IUserRepository _userRepository;
    protected readonly IChatsIntegrationEventsHub IntegrationEventsHub;
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    protected BaseChatService(
        IChatRepository<TChatMessage> repository,
        IChatsIntegrationEventsHub integrationEventsHub,
        IUserRepository userRepository,
        INotificationService notificationService,
        IMapper mapper)
    {
        _repository = repository;
        IntegrationEventsHub = integrationEventsHub;
        _userRepository = userRepository;
        _notificationService = notificationService;
        _mapper = mapper;
    }

    public Task<Result<TChatMessage>> GetAsync(long authorizedUserId, Guid messageId)
        => _repository.GetMessageAsync(messageId);

    public async Task<Result<BaseCollection<TChatMessage>>> GetListAsync(long chatId, int offset = 0, int limit = 300)
    {
        var messages = await _repository.GetMessagesAsync(chatId, offset, limit);
        return Result<BaseCollection<TChatMessage>>.FromValue(messages);
    }

    protected abstract Task<Result> ValidateAsync(TNewChatMessage message);

    protected async Task<Result> SendAsync(long ownerId, TNewChatMessage newChatMessage)
    {
        var validationResult = await ValidateAsync(newChatMessage);
        if (!validationResult.IsSuccess)
            return validationResult;

        var typedChatMessage = await GetTypedChatMessage<TChatMessage>(ownerId, newChatMessage);

        typedChatMessage.Timestamp = DateTime.UtcNow;

        var messageId = await _repository.AddMessageAsync(typedChatMessage);

        await PublicIntegrationEvent(messageId, newChatMessage);

        await NotifyUsersIfNeed(ownerId, newChatMessage);

        return Result.Success;
    }

    protected abstract Task PublicIntegrationEvent(Guid messageId, TNewChatMessage newChatMessage);

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

    Task<Result> IChatService<TNewChatMessage, TChatMessage>.SendAsync(long ownerId, TNewChatMessage newMessage)
        => SendAsync(ownerId, newMessage);
}
