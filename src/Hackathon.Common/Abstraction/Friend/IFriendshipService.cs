using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Friend;
using System.Threading.Tasks;
using Hackathon.Common.Models.Users;

namespace Hackathon.Common.Abstraction.Friend;

public interface IFriendshipService
{
    /// <summary>
    /// Получить список предложений дружбы
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="parameters"></param>
    Task<BaseCollection<Friendship>> GetOffersAsync(long userId, Common.Models.GetListParameters<FriendshipGetOffersFilter> parameters);

    /// <summary>
    /// Создать или принять предложение дружбы
    /// </summary>
    Task<Result> CreateOrAcceptOfferAsync(long proposerId, long userId);

    /// <summary>
    /// Отклонить предложение дружбы
    /// </summary>
    Task<Result> RejectOfferAsync(long userId, long proposerId);

    /// <summary>
    /// Отписаться от пользователя
    /// </summary>
    /// <param name="proposerId">Инициатор отписки</param>
    /// <param name="userId">Идентификатор пользователя от которого надо отписаться</param>
    Task<Result> UnsubscribeAsync(long proposerId, long userId);

    /// <summary>
    /// Завершить дружбу с пользователем
    /// </summary>
    /// <param name="firstUserId"></param>
    /// <param name="secondUserId"></param>
    Task<Result> EndFriendship(long firstUserId, long secondUserId);

    /// <summary>
    /// Получить список пользователей по статусу дружбы
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="status"></param>
    Task<Result<BaseCollection<UserModel>>> GetUsersByFriendshipStatus(long userId, FriendshipStatus status);
}
