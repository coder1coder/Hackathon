using System;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Common.Abstraction.Friend;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Common.Abstraction.Notification;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.ErrorMessages;
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

    public Task<BaseCollection<Friendship>> GetOffersAsync(
        long userId, Common.Models.GetListParameters<FriendshipGetOffersFilter> parameters)
        => _friendshipRepository.GetOffersAsync(userId, parameters);

    public async Task<Result> CreateOrAcceptOfferAsync(long proposerId, long userId)
    {
        if (proposerId == userId)
            return Result.NotFound(FriendshipMessages.CantCreateAcceptFriendshipOffersForYourProfile);

        var offer = await _friendshipRepository.GetOfferAsync(proposerId, userId);

        if (offer?.Status == FriendshipStatus.Confirmed)
            return Result.NotFound(FriendshipMessages.UsersAreAlreadyFriends);

        var proposer = await _userService.GetAsync(proposerId);

        //Если предложение не существует
        if (offer is null)
        {
            await _friendshipRepository.CreateOfferAsync(proposerId, userId);
            await _notificationService.PushAsync(CreateNotificationModel
                .Information(userId, $"Запрос дружбы от {proposer}", proposerId));

            await _messageHub.Publish(TopicNames.FriendshipChanged,
                new FriendshipChangedIntegrationEvent(new []{ proposerId, userId }));

            return Result.Success;
        }

        //Если предложение дружбы создано пользователем запроса
        if (offer.ProposerId == proposerId)
        {
            switch (offer.Status)
            {
                case FriendshipStatus.Pending:
                    return Result.NotValid(FriendshipMessages.OfferOfFriendshipWasSentEarlier);

                //Предложение было отклонено ранее, обновим статус
                case FriendshipStatus.Rejected:
                    await _friendshipRepository.UpdateStatusAsync(proposerId, userId, FriendshipStatus.Pending);
                    await _notificationService.PushAsync(CreateNotificationModel
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
                await _notificationService.PushAsync(CreateNotificationModel
                    .Information(userId, $"{proposer} принял предложение дружбы", proposerId));

                await _messageHub.Publish(TopicNames.FriendshipChanged,
                    new FriendshipChangedIntegrationEvent(new []{ proposerId, userId }));
            }
        }

        return Result.Success;
    }

    public async Task<Result> RejectOfferAsync(long userId, long proposerId)
    {
        var offer = await _friendshipRepository.GetOfferAsync(proposerId, userId, GetOfferOption.Outgoing);
        if (offer is null)
            return Result.NotValid(FriendshipMessages.NoOffersOfFriendshipHaveBeenMade);

        if (offer.Status != FriendshipStatus.Pending)
            return Result.NotValid(FriendshipMessages.InappropriateStatus);

        await _friendshipRepository.UpdateStatusAsync(proposerId, userId, FriendshipStatus.Rejected);

        //TODO: отправлять событие в шину, и вынести логику уведомления в хендлер
        var user = await _userService.GetAsync(userId);
        await _notificationService.PushAsync(CreateNotificationModel
            .Information(userId, $"{user} отклонил предложение дружбы"));

        await _messageHub.Publish(TopicNames.FriendshipChanged,
            new FriendshipChangedIntegrationEvent(new []{ proposerId, userId }));

        return Result.Success;
    }

    public async Task<Result> UnsubscribeAsync(long proposerId, long userId)
    {
        var offer = await _friendshipRepository.GetOfferAsync(proposerId, userId, GetOfferOption.Outgoing);
        if (offer is not { Status: FriendshipStatus.Pending })
            return Result.NotValid(FriendshipMessages.YouAreNotSubscribedToUser);

        await _friendshipRepository.RemoveOfferAsync(proposerId, userId);

        await _messageHub.Publish(TopicNames.FriendshipChanged,
            new FriendshipChangedIntegrationEvent(new []{ proposerId, userId }));

        return Result.Success;
    }

    public async Task<Result> EndFriendship(long firstUserId, long secondUserId)
    {
        var offer = await _friendshipRepository.GetOfferAsync(firstUserId, secondUserId);

        if (offer is not { Status: FriendshipStatus.Confirmed })
            return Result.NotValid(FriendshipMessages.UsersAreNotFriends);

        await _friendshipRepository.UpdateFriendship(offer.ProposerId, offer.UserId, new Friendship
        {
            ProposerId = secondUserId,
            UserId = firstUserId,
            Status = FriendshipStatus.Pending
        });

        await _messageHub.Publish(TopicNames.FriendshipChanged,
            new FriendshipChangedIntegrationEvent(new []{ firstUserId, secondUserId }));

        return Result.Success;
    }

    public async Task<Result<BaseCollection<UserModel>>> GetUsersByFriendshipStatus(long userId, FriendshipStatus status)
        => Result<BaseCollection<UserModel>>.FromValue(await _friendshipRepository.GetUsersByFriendshipStatus(userId, status));
}
