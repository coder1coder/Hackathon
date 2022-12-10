using System;
using System.Threading.Tasks;
using Hackathon.Abstraction.Friend;
using Hackathon.Abstraction.IntegrationEvents;
using Hackathon.Abstraction.Notification;
using Hackathon.Abstraction.User;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Friend;
using Hackathon.Common.Models.Notification;
using Hackathon.Common.Models.User;
using Hackathon.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvent;

namespace Hackathon.BL.Friend;

public class FriendshipService: IFriendshipService
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly INotificationService _notificationService;
    private readonly IUserService _userService;
    private readonly IMessageHub<FriendshipChangedIntegrationEvent> _messageHub;

    public FriendshipService(
        IFriendshipRepository friendshipRepository,
        INotificationService notificationService,
        IUserService userService,
        IMessageHub<FriendshipChangedIntegrationEvent> messageHub)
    {
        _friendshipRepository = friendshipRepository;
        _notificationService = notificationService;
        _userService = userService;
        _messageHub = messageHub;
    }

    public async Task<BaseCollection<Friendship>> GetOffersAsync(
        long userId,
        GetListParameters<FriendshipGetOffersFilter> parameters)
        => await _friendshipRepository.GetOffersAsync(userId, parameters);

    public async Task CreateOrAcceptOfferAsync(long proposerId, long userId)
    {
        if (proposerId == userId)
            throw new ValidationException("Нельзя создать/принять предложение дружбы для своего профиля");

        var offer = await _friendshipRepository.GetOfferAsync(proposerId, userId);

        if (offer?.Status == FriendshipStatus.Confirmed)
            throw new ValidationException("Пользователи уже являются друзьями");

        var proposer = await _userService.GetAsync(proposerId);

        //Если предложение не существует
        if (offer == null)
        {
            await _friendshipRepository.CreateOfferAsync(proposerId, userId);
            await _notificationService.Push(CreateNotificationModel
                .Information(userId, $"Запрос дружбы от {proposer}", proposerId));

            await _messageHub.Publish(TopicNames.FriendshipChanged,
                new FriendshipChangedIntegrationEvent(new []{ proposerId, userId }));

            return;
        }

        //Если предложение дружбы создано пользователем запроса
        if (offer.ProposerId == proposerId)
        {
            switch (offer.Status)
            {
                case FriendshipStatus.Pending:
                    throw new ValidationException("Предложение дружбы было отправлено ранее");

                //Предложение было отклонено ранее, обновим статус
                case FriendshipStatus.Rejected:
                    await _friendshipRepository.UpdateStatusAsync(proposerId, userId, FriendshipStatus.Pending);
                    await _notificationService.Push(CreateNotificationModel
                        .Information(userId, $"Запрос дружбы от {proposer}", proposerId));

                    await _messageHub.Publish(TopicNames.FriendshipChanged,
                        new FriendshipChangedIntegrationEvent(new []{ proposerId, userId }));

                    break;
                case FriendshipStatus.Confirmed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        //Если предложение дружбы создано другим пользователем
        else
        {
            //Создавая встречное предложение мы принимаем существующее
            if (offer.Status == FriendshipStatus.Pending)
            {
                await _friendshipRepository.UpdateStatusAsync(userId, proposerId, FriendshipStatus.Confirmed);
                await _notificationService.Push(CreateNotificationModel
                    .Information(userId, $"{proposer} принял предложение дружбы", proposerId));

                await _messageHub.Publish(TopicNames.FriendshipChanged,
                    new FriendshipChangedIntegrationEvent(new []{ proposerId, userId }));
            }
        }
    }

    public async Task RejectOfferAsync(long userId, long proposerId)
    {
        var offer = await _friendshipRepository.GetOfferAsync(proposerId, userId, GetOfferOption.Outgoing);
        if (offer == null)
            throw new ValidationException("Предложений дружбы не поступало");

        if (offer.Status != FriendshipStatus.Pending)
            throw new ValidationException("Не подходящий статус");

        await _friendshipRepository.UpdateStatusAsync(proposerId, userId, FriendshipStatus.Rejected);

        //TODO: отправлять событие в шину, и вынести логику уведомления в хендлер
        var user = await _userService.GetAsync(userId);
        await _notificationService.Push(CreateNotificationModel
            .Information(userId, $"{user} отклонил предложение дружбы"));

        await _messageHub.Publish(TopicNames.FriendshipChanged,
            new FriendshipChangedIntegrationEvent(new []{ proposerId, userId }));
    }

    public async Task UnsubscribeAsync(long proposerId, long userId)
    {
        var offer = await _friendshipRepository.GetOfferAsync(proposerId, userId, GetOfferOption.Outgoing);
        if (offer is not { Status: FriendshipStatus.Pending })
            throw new ValidationException("Вы не подписаны на пользователя");

        await _friendshipRepository.RemoveOfferAsync(proposerId, userId);

        await _messageHub.Publish(TopicNames.FriendshipChanged,
            new FriendshipChangedIntegrationEvent(new []{ proposerId, userId }));
    }

    public async Task EndFriendship(long firstUserId, long secondUserId)
    {
        var offer = await _friendshipRepository.GetOfferAsync(firstUserId, secondUserId);

        if (offer is not { Status: FriendshipStatus.Confirmed })
            throw new ValidationException("Пользователи не являются друзьями");

        await _friendshipRepository.UpdateFriendship(offer.ProposerId, offer.UserId, new Friendship
        {
            ProposerId = secondUserId,
            UserId = firstUserId,
            Status = FriendshipStatus.Pending
        });

        await _messageHub.Publish(TopicNames.FriendshipChanged,
            new FriendshipChangedIntegrationEvent(new []{ firstUserId, secondUserId }));
    }

    public Task<BaseCollection<UserModel>> GetUsersByFriendshipStatus(long userId, FriendshipStatus status)
        => _friendshipRepository.GetUsersByFriendshipStatus(userId, status);
}
