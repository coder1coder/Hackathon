using Hackathon.Common.Abstraction.User;
using System;
using System.Threading.Tasks;
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

[SwaggerTag("Пользователи системы")]
public class UserController: BaseController
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IEmailConfirmationService _emailConfirmationService;

    /// <summary>
    /// Пользователи
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="userService"></param>
    /// <param name="emailConfirmationService"></param>
    public UserController(
        IMapper mapper,
        IUserService userService,
        IEmailConfirmationService emailConfirmationService)
    {
        _mapper = mapper;
        _userService = userService;
        _emailConfirmationService = emailConfirmationService;
    }

    /// <summary>
    /// Создание нового пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<BaseCreateResponse> SignUp([FromBody] SignUpRequest request)
    {
        var signUpModel = _mapper.Map<SignUpModel>(request);
        var userId = await _userService.CreateAsync(signUpModel);
        return new BaseCreateResponse
        {
            Id = userId
        };
    }

    /// <summary>
    /// Получить информацию о пользователе
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    [HttpGet("{userId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    public async Task<IActionResult> GetAsync([FromRoute] long userId)
    {
        var getUserResult = await _userService.GetAsync(userId);

        if (!getUserResult.IsSuccess)
            return await GetResult(() => Task.FromResult(getUserResult));

        return Ok(_mapper.Map<UserResponse>(getUserResult.Data));
    }

    /// <summary>
    /// Получить информацию о пользователях
    /// </summary>
    /// <returns></returns>
    [HttpPost("list")]
    [Authorize(Policy = nameof(UserRole.Administrator))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseCollection<UserResponse>))]
    public async Task<IActionResult> GetListAsync([FromBody] GetListParameters<UserFilter> request)
        => Ok(await _userService.GetAsync(request));

    /// <summary>
    /// Загрузить картинку профиля пользователя
    /// </summary>
    /// <param name="file">Файл изображения</param>
    /// <returns></returns>
    [HttpPost("profile/image/upload")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UploadProfileImage(IFormFile file)
    {
        var uploadProfileImageResult = await _userService.UploadProfileImageAsync(AuthorizedUserId, file);
        if (!uploadProfileImageResult.IsSuccess)
            return await GetResult(() => Task.FromResult(uploadProfileImageResult));

        return Ok(uploadProfileImageResult.Data);
    }

    /// <summary>
    /// Подтвердить Email
    /// </summary>
    /// <param name="code">Код подтверждения</param>
    [HttpPost("profile/email/confirm")]
    public Task ConfirmEmail([FromQuery] string code)
        => GetResult(() => _emailConfirmationService.Confirm(AuthorizedUserId, code));

    /// <summary>
    /// Создать запрос на подтвреждение Email пользователя
    /// </summary>
    /// <returns></returns>
    [HttpPost("profile/email/confirm/request")]
    public Task<IActionResult> CreateEmailConfirmationRequest()
        => GetResult(() => _emailConfirmationService.CreateRequest(AuthorizedUserId));

    /// <summary>
    /// Обновить профиль пользователя
    /// </summary>
    /// <param name="request"></param>
    [HttpPut("profile/update")]
    public Task<IActionResult> UpdateUserProfile(UpdateUserRequest request)
        => GetResult(() => _userService.UpdateUserAsync(_mapper.Map<UpdateUserParameters>(request)));
}
