using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Friend;

namespace Hackathon.Abstraction.Friend;

public interface IFriendshipService
{
    /// <summary>
    /// Получить список предложений дружбы
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<BaseCollection<Friendship>> GetOffersAsync(long userId, GetListParameters<FriendshipGetOffersFilter> parameters);

    /// <summary>
    /// Создать или принять предложение дружбы
    /// </summary>
    Task CreateOrAcceptOfferAsync(long proposerId, long userId);

    /// <summary>
    /// Отклонить предложение дружбы
    /// </summary>
    Task RejectOfferAsync(long userId, long proposerId);

    /// <summary>
    /// Завершить дружбу с пользователем
    /// </summary>
    /// <param name="firstUserId"></param>
    /// <param name="secondUserId"></param>
    /// <returns></returns>
    Task EndFriendship(long firstUserId, long secondUserId);
}