using System.Threading.Tasks;
using Hackathon.Abstraction.User;
using Hackathon.Common.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[Route("api/User")]
public class UserProfileReactionController: BaseController
{
    private readonly IUserProfileReactionService _userProfileReactionService;

    public UserProfileReactionController(IUserProfileReactionService userProfileReactionService)
    {
        _userProfileReactionService = userProfileReactionService;
    }

    /// <summary>
    /// Получить все реакции на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получившего реакцию</param>
    /// <returns></returns>
    [HttpGet("{userId:long}/reactions")]
    public async Task<UserProfileReaction?> GetReactions(long userId)
        => await _userProfileReactionService.GetReactionsAsync(UserId, userId);

    /// <summary>
    /// Добавить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получающего реакцию</param>
    /// <param name="reaction">Реакция</param>
    [HttpPost("{userId:long}/reactions/{reaction}")]
    public async Task AddReaction(long userId, UserProfileReaction reaction)
        => await _userProfileReactionService.UpsertReactionAsync(UserId, userId, reaction);

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получившего реакцию</param>
    /// <param name="reaction">Реакция</param>
    [HttpDelete("{userId:long}/reactions/{reaction}")]
    public async Task RemoveReaction(long userId, UserProfileReaction reaction)
        => await _userProfileReactionService.RemoveReactionAsync(UserId, userId, reaction);
}
