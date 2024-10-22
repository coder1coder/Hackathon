using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Friend;
using Refit;

namespace Hackathon.Client;

public interface IFriendshipApi
{
    private const string BaseRoute = "/api/friendship";

    [Post(BaseRoute + "/offers/list")]
    Task<BaseCollection<Friendship>> GetOffers([Body] GetListParameters<FriendshipGetOffersFilter> parameters);

    /// <summary>
    /// Создать или принять предложение дружбы
    /// </summary>
    [Post(BaseRoute + "/offer/{userId}")]
    Task CreateOrAcceptOffer(long userId);

    /// <summary>
    /// Отклонить предложение дружбы
    /// </summary>
    /// <param name="proposerId">Инициатор предложения</param>
    [Post(BaseRoute + "/offer/reject/{proposerId}")]
    Task RejectOffer(long proposerId);

    /// <summary>
    /// Прекратить дружбу
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [Delete(BaseRoute + "/{userId}")]
    Task EndFriendship(long userId);
}
