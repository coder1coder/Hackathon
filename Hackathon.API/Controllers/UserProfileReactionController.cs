using Hackathon.Common.Abstraction.User;
using System.Threading.Tasks;
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
    public Task<UserProfileReaction?> GetReactions(long userId)
        => _userProfileReactionService.GetReactionsAsync(AuthorizedUserId, userId);

    /// <summary>
    /// Добавить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получающего реакцию</param>
    /// <param name="reaction">Реакция</param>
    [HttpPost("{userId:long}/reactions/{reaction}")]
    public Task AddReaction(long userId, UserProfileReaction reaction)
        => _userProfileReactionService.UpsertReactionAsync(AuthorizedUserId, userId, reaction);

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получившего реакцию</param>
    /// <param name="reaction">Реакция</param>
    [HttpDelete("{userId:long}/reactions/{reaction}")]
    public Task RemoveReaction(long userId, UserProfileReaction reaction)
        => _userProfileReactionService.RemoveReactionAsync(AuthorizedUserId, userId, reaction);
}
