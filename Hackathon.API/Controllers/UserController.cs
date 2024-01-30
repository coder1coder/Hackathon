using Hackathon.Common.Abstraction.User;
using System;
using System.Threading.Tasks;
using Hackathon.API.Module;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.User;
using Hackathon.Contracts.Responses;
using Hackathon.Contracts.Responses.User;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

/// <summary>
/// Пользователи
/// </summary>
[SwaggerTag("Пользователи системы")]
public class UserController(
    IMapper mapper,
    IUserService userService,
    IEmailConfirmationService emailConfirmationService)
    : BaseController
{
    /// <summary>
    /// Создание нового пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<BaseCreateResponse> SignUp([FromBody] SignUpRequest request)
    {
        var signUpModel = mapper.Map<CreateNewUserModel>(request);
        var userId = await userService.CreateAsync(signUpModel);
        return new BaseCreateResponse
        {
            Id = userId
        };
    }

    /// <summary>
    /// Обновить пароль пользователя
    /// </summary>
    /// <param name="parameters">Параметры</param>
    /// <returns></returns>
    [HttpPut("password")]
    public Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel parameters)
        => GetResult(() => userService.UpdatePasswordAsync(AuthorizedUserId, parameters)); 

    /// <summary>
    /// Получить информацию о пользователе
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    [HttpGet("{userId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    public async Task<IActionResult> GetAsync([FromRoute] long userId)
    {
        var getUserResult = await userService.GetAsync(userId);

        if (!getUserResult.IsSuccess)
            return await GetResult(() => Task.FromResult(getUserResult));

        return Ok(mapper.Map<UserResponse>(getUserResult.Data));
    }

    /// <summary>
    /// Получить информацию о пользователях
    /// </summary>
    /// <returns></returns>
    [HttpPost("list")]
    [Authorize(Policy = nameof(UserRole.Administrator))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseCollection<UserResponse>))]
    public async Task<IActionResult> GetListAsync([FromBody] GetListParameters<UserFilter> request)
        => Ok(await userService.GetListAsync(request));

    /// <summary>
    /// Загрузить картинку профиля пользователя
    /// </summary>
    /// <param name="file">Файл изображения</param>
    /// <returns></returns>
    [HttpPost("profile/image/upload")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public Task<IActionResult> UploadProfileImage(IFormFile file)
        => GetResult(() => userService.UploadProfileImageAsync(AuthorizedUserId, file));

    /// <summary>
    /// Подтвердить Email
    /// </summary>
    /// <param name="code">Код подтверждения</param>
    [HttpPost("profile/email/confirm")]
    public Task ConfirmEmail([FromQuery] string code)
        => GetResult(() => emailConfirmationService.Confirm(AuthorizedUserId, code));

    /// <summary>
    /// Создать запрос на подтвреждение Email пользователя
    /// </summary>
    /// <returns></returns>
    [HttpPost("profile/email/confirm/request")]
    public Task<IActionResult> CreateEmailConfirmationRequest()
        => GetResult(() => emailConfirmationService.CreateRequest(AuthorizedUserId));

    /// <summary>
    /// Обновить профиль пользователя
    /// </summary>
    /// <param name="request"></param>
    [HttpPut("profile/update")]
    public Task<IActionResult> UpdateUserProfile(UpdateUserRequest request)
        => GetResult(() => userService.UpdateUserAsync(mapper.Map<UpdateUserParameters>(request)));
}
