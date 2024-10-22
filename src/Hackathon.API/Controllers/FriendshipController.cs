using Hackathon.Common.Abstraction.Friend;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Friend;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Hackathon.API.Module;
using Hackathon.Common.Models.Users;

namespace Hackathon.API.Controllers;

[SwaggerTag("Друзья")]
public class FriendshipController : BaseController
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
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(BaseCollection<UserModel>))]
    public Task<IActionResult> GetUsersByFriendshipStatus(
        [FromQuery, Required] long userId,
        [FromQuery, Required] FriendshipStatus status)
        => GetResult(() => _friendshipService.GetUsersByFriendshipStatus(userId, status));

    /// <summary>
    /// Получить список предложений дружбы
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [HttpPost("offers/list")]
    public Task<BaseCollection<Friendship>> GetOffers([FromBody] GetListParameters<FriendshipGetOffersFilter> parameters)
        => _friendshipService.GetOffersAsync(AuthorizedUserId, parameters);

    /// <summary>
    /// Создать или принять предложение дружбы
    /// </summary>
    [HttpPost("offer/{userId:long}")]
    public Task<IActionResult> CreateOrAcceptOffer([FromRoute, Required] long userId)
        => GetResult(() => _friendshipService.CreateOrAcceptOfferAsync(AuthorizedUserId, userId));

    /// <summary>
    /// Отклонить предложение дружбы
    /// </summary>
    /// <param name="proposerId">Инициатор предложения</param>
    [HttpPost("offer/reject/{proposerId:long}")]
    public Task<IActionResult> RejectOffer([FromRoute, Required] long proposerId)
        => GetResult(() => _friendshipService.RejectOfferAsync(AuthorizedUserId, proposerId));

    /// <summary>
    /// Отписаться от профиля пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    [HttpPost("unsubscribe/{userId:long}")]
    public Task<IActionResult> Unsubscribe([FromRoute] long userId)
        => GetResult(() => _friendshipService.UnsubscribeAsync(AuthorizedUserId, userId));

    /// <summary>
    /// Прекратить дружбу
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{userId:long}")]
    public Task<IActionResult> EndFriendship(long userId)
        => GetResult(() => _friendshipService.EndFriendship(AuthorizedUserId, userId));
}
