using Hackathon.Common.Abstraction.User;
using System.Threading.Tasks;
using Hackathon.API.Module;
using Hackathon.Common.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

[Route("api/user/{userId:long}/reactions")]
[SwaggerTag("Реакции на профиль пользователя")]
public class UserProfileReactionController : BaseController
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
    [HttpGet]
    public Task<IActionResult> GetReactions(long userId)
        => GetResult(() => _userProfileReactionService.GetReactionsAsync(AuthorizedUserId, userId));

    /// <summary>
    /// Получить реакции на профиль пользователя по типу с количеством, поставленные другими пользователями
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получившего реакции</param>
    [HttpGet("count")]
    public Task<IActionResult> GetReactionsByType(long userId)
        => GetResult(() => _userProfileReactionService.GetReactionsByTypeAsync(userId));

    /// <summary>
    /// Добавить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получающего реакцию</param>
    /// <param name="reaction">Реакция</param>
    [HttpPost("{reaction}")]
    public Task<IActionResult> AddReaction(long userId, UserProfileReaction reaction)
        => GetResult(() => _userProfileReactionService.UpsertReactionAsync(AuthorizedUserId, userId, reaction));

    /// <summary>
    /// Удалить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получившего реакцию</param>
    /// <param name="reaction">Реакция</param>
    [HttpDelete("{reaction}")]
    public Task<IActionResult> RemoveReaction(long userId, UserProfileReaction reaction)
        => GetResult(() => _userProfileReactionService.RemoveReactionAsync(AuthorizedUserId, userId, reaction));
}
