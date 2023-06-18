using Hackathon.Common.Abstraction.User;
using System.Threading.Tasks;
using Hackathon.Common.Models.User;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

[Route("api/user/{userId:long}/reactions")]
[SwaggerTag("Реакции на профиль пользователя")]
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
    [HttpGet]
    public Task<UserProfileReaction?> GetReactions(long userId)
        => _userProfileReactionService.GetReactionsAsync(AuthorizedUserId, userId);

    /// <summary>
    /// Добавить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получающего реакцию</param>
    /// <param name="reaction">Реакция</param>
    [HttpPost("{reaction}")]
    public Task AddReaction(long userId, UserProfileReaction reaction)
        => _userProfileReactionService.UpsertReactionAsync(AuthorizedUserId, userId, reaction);

    /// <summary>
    /// Удалить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получившего реакцию</param>
    /// <param name="reaction">Реакция</param>
    [HttpDelete("{reaction}")]
    public Task RemoveReaction(long userId, UserProfileReaction reaction)
        => _userProfileReactionService.RemoveReactionAsync(AuthorizedUserId, userId, reaction);
}
