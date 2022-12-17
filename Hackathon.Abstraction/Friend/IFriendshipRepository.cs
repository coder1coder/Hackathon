using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Friend;
using Hackathon.Common.Models.User;

namespace Hackathon.Abstraction.Friend;

public interface IFriendshipRepository
{
    /// <summary>
    /// Получить список пользователей по статусу дружбы
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    Task<BaseCollection<UserModel>> GetUsersByFriendshipStatus(long userId, FriendshipStatus status);

    /// <summary>
    /// Добавить или принять предложение дружбы
    /// </summary>
    Task CreateOfferAsync(long proposerId, long userId);

    /// <summary>
    /// Обновляет статус предложения дружбы
    /// </summary>
    /// <param name="proposerId">Инициатор предложения</param>
    /// <param name="userId">Пользователь</param>
    /// <param name="status">Статус</param>
    /// <returns></returns>
    Task UpdateStatusAsync(long proposerId, long userId, FriendshipStatus status);

    Task UpdateFriendship(long proposerId, long userId, Friendship parameters);

    /// <summary>
    /// Получить предложение дружбы
    /// </summary>
    /// <param name="proposerId"></param>
    /// <param name="userId"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    Task<Friendship> GetOfferAsync(long proposerId, long userId, GetOfferOption option = GetOfferOption.Any);

    /// <summary>
    /// Получить список предложений дружбы
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<BaseCollection<Friendship>> GetOffersAsync(long userId,
        GetListParameters<FriendshipGetOffersFilter> parameters);

    /// <summary>
    /// Удалить предложение дружбы
    /// </summary>
    /// <param name="proposerId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task RemoveOfferAsync(long proposerId, long userId);
}
