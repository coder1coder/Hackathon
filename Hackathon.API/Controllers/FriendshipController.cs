using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hackathon.Abstraction.Friend;
using Hackathon.API.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Friend;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

[SwaggerTag("Друзья")]
public class FriendshipController: BaseController, IFriendshipApi
{
    private readonly IFriendshipService _friendshipService;

    public FriendshipController(IFriendshipService friendshipService)
    {
        _friendshipService = friendshipService;
    }

    /// <summary>
    /// Получить список пользователей по статусу дружбы
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpGet("users")]
    public Task<BaseCollection<UserModel>> GetUsersByFriendshipStatus(
        [FromQuery, Required] long userId,
        [FromQuery, Required] FriendshipStatus status)
        => _friendshipService.GetUsersByFriendshipStatus(userId, status);

    /// <summary>
    /// Получить список предложений дружбы
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [HttpPost("offers/list")]
    public async Task<BaseCollectionResponse<Friendship>> GetOffers([FromBody] GetListParameters<FriendshipGetOffersFilter> parameters)
    {
        var offers = await _friendshipService.GetOffersAsync(UserId, parameters);

        return new BaseCollectionResponse<Friendship>
        {
            Items = offers.Items,
            TotalCount = offers.TotalCount
        };
    }

    /// <summary>
    /// Создать или принять предложение дружбы
    /// </summary>
    [HttpPost("offer/{userId:long}")]
    public async Task CreateOrAcceptOffer([FromRoute, Required] long userId)
        => await _friendshipService.CreateOrAcceptOfferAsync(UserId, userId);

    /// <summary>
    /// Отклонить предложение дружбы
    /// </summary>
    /// <param name="proposerId">Инициатор предложения</param>
    [HttpPost("offer/reject/{proposerId:long}")]
    public async Task RejectOffer([FromRoute, Required] long proposerId)
        => await _friendshipService.RejectOfferAsync(UserId, proposerId);

    /// <summary>
    /// Отписаться от профиля пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    [HttpPost("unsubscribe/{userId:long}")]
    public async Task Unsubscribe([FromRoute] long userId)
        => await _friendshipService.UnsubscribeAsync(UserId, userId);

    /// <summary>
    /// Прекратить дружбу
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{userId:long}")]
    public async Task EndFriendship(long userId)
        => await _friendshipService.EndFriendship(UserId, userId);
}
