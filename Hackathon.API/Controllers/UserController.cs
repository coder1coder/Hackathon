using System;
using System.Threading.Tasks;
using Hackathon.Abstraction.User;
using Hackathon.Common.Models;
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
        var userResult = await _userService.GetAsync(userId);

        if (userResult.IsSuccess)
            return Ok(_mapper.Map<UserResponse>(userResult.Data));

        return await GetResult(() => Task.FromResult(userResult));
    }

    /// <summary>
    /// Получить информацию о пользователях
    /// </summary>
    /// <returns></returns>
    [HttpPost("list")]
    [Authorize(Policy = nameof(UserRole.Administrator))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseCollectionResponse<UserResponse>))]
    public async Task<IActionResult> GetListAsync([FromBody] GetListParameters<UserFilter> request)
    {
        var collectionModel = await _userService.GetAsync(request);
        return Ok(_mapper.Map<BaseCollectionResponse<UserResponse>>(collectionModel));
    }

    /// <summary>
    /// Загрузить картинку профиля пользователя.
    /// </summary>
    /// <param name="file">Файл картинка.</param>
    /// <returns></returns>
    [HttpPost("profile/image/upload")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UploadProfileImage(IFormFile file)
    {
        //TODO: вынести в BL
        if (file is null)
            return BadRequest("Файл не может быть пустым.");

        await using var stream = file.OpenReadStream();

        var newProfileImageId = await _userService.UploadProfileImageAsync(UserId, file.FileName, stream);

        return Ok(newProfileImageId);
    }

    /// <summary>
    /// Подтвердить Email
    /// </summary>
    /// <param name="code">Код подтверждения</param>
    [HttpPost("profile/email/confirm")]
    public Task ConfirmEmail([FromQuery] string code)
        => GetResult(() => _emailConfirmationService.Confirm(UserId, code));

    /// <summary>
    /// Создать запрос на подтвреждение Email пользователя
    /// </summary>
    /// <returns></returns>
    [HttpPost("profile/email/confirm/request")]
    public Task<IActionResult> CreateEmailConfirmationRequest()
        => GetResult(() => _emailConfirmationService.CreateRequest(UserId));
}
