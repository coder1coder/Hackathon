using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hackathon.Abstraction.Friend;
using Hackathon.API.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Friend;
using Hackathon.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

[SwaggerTag("Друзья")]
public class FriendController: BaseController, IFriendshipApi
{
    private readonly IFriendshipService _friendshipService;
    
    public FriendController(IFriendshipService friendshipService)
    {
        _friendshipService = friendshipService;
    }

    /// <summary>
    /// Получить список предложений дружбы
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("offers/list")]
    public async Task<BaseCollectionResponse<Friendship>> GetOffers([FromBody] GetListParameters<FriendshipGetOffersFilter> request)
    {
        var offers = await _friendshipService.GetOffersAsync(UserId, request);

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
    /// Прекратить дружбу
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{userId:long}")]
    public async Task EndFriendship(long userId)
        => await _friendshipService.EndFriendship(UserId, userId);
}