using Hackathon.Common.Abstraction.User;
using System.Threading.Tasks;
using Hackathon.API.Module;
using Hackathon.Common.Models.User;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

[Route("api/user/{userId:long}/reactions")]
[SwaggerTag("Реакции на профиль пользователя")]
public class UserProfileReactionController(IUserProfileReactionService userProfileReactionService) : BaseController
{
    /// <summary>
    /// Получить все реакции на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получившего реакцию</param>
    /// <returns></returns>
    [HttpGet]
    public Task<IActionResult> GetReactions(long userId)
        => GetResult(() => userProfileReactionService.GetReactionsAsync(AuthorizedUserId, userId));

    /// <summary>
    /// Получить реакции на профиль пользователя по типу с количеством, поставленные другими пользователями
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получившего реакции</param>
    /// <returns></returns>
    [HttpGet("count")]
    public Task<IActionResult> GetReactionsByType(long userId)
        => GetResult(() => userProfileReactionService.GetReactionsByTypeAsync(userId));

    /// <summary>
    /// Добавить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получающего реакцию</param>
    /// <param name="reaction">Реакция</param>
    [HttpPost("{reaction}")]
    public Task<IActionResult> AddReaction(long userId, UserProfileReaction reaction)
        => GetResult(() => userProfileReactionService.UpsertReactionAsync(AuthorizedUserId, userId, reaction));

    /// <summary>
    /// Удалить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получившего реакцию</param>
    /// <param name="reaction">Реакция</param>
    [HttpDelete("{reaction}")]
    public Task<IActionResult> RemoveReaction(long userId, UserProfileReaction reaction)
        => GetResult(() => userProfileReactionService.RemoveReactionAsync(AuthorizedUserId, userId, reaction));
}
