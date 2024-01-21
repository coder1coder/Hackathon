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
using Hackathon.Common.Models.Block;
using Hackathon.Contracts.Requests.Block;
using System.Net;
using Hackathon.Common.Abstraction.Block;

namespace Hackathon.API.Controllers;

[SwaggerTag("Пользователи системы")]
public class UserController: BaseController
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IEmailConfirmationService _emailConfirmationService;
    private readonly IBlockingService _blockingService;

    /// <summary>
    /// Пользователи
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="userService"></param>
    /// <param name="emailConfirmationService"></param>
    /// <param name="blockingService"></param>
    public UserController(
        IMapper mapper,
        IUserService userService,
        IEmailConfirmationService emailConfirmationService,
        IBlockingService blockingService)
    {
        _mapper = mapper;
        _userService = userService;
        _emailConfirmationService = emailConfirmationService;
        _blockingService = blockingService;
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
        => Ok(await _userService.GetListAsync(request));

    /// <summary>
    /// Загрузить картинку профиля пользователя
    /// </summary>
    /// <param name="file">Файл изображения</param>
    /// <returns></returns>
    [HttpPost("profile/image/upload")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public Task<IActionResult> UploadProfileImage(IFormFile file)
        => GetResult(() => _userService.UploadProfileImageAsync(AuthorizedUserId, file));

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

    /// <summary>
    /// Создать новую блокировку
    /// </summary>
    /// <param name="targetUserId">Id пользователя, на которого назначена блокировка</param>
    /// <param name="createBlockRequest"></param>
    /// <returns></returns>
    [HttpPost("blocking/{targetUserId:long}")]
    [ProducesResponseType(typeof(BaseCreateResponse), (int)HttpStatusCode.OK)]
    public Task<IActionResult> CreateBlockingAsync(long targetUserId, [FromBody] CreateBlockRequest createBlockRequest)
        => GetResult(() => _blockingService.CreateAsync(new BlockingCreateParameters
        {
            TargetUserId = targetUserId,
            ActionDate = createBlockRequest.ActionDate,
            ActionHours = createBlockRequest.ActionHours,
            Reason = createBlockRequest.Reason,
            Type = createBlockRequest.Type,
            AssignmentUserId = AuthorizedUserId
        }));

    /// <summary>
    /// Удалить блокировку
    /// </summary>
    /// <param name="targetUserId">Id пользователя, с которого снимается блокировка</param>
    /// <returns></returns>
    [HttpDelete("blocking/{targetUserId:long}")]
    public Task<IActionResult> DeleteBlockingAsync(long targetUserId)
        => GetResult(() => _blockingService.DeleteAsync(AuthorizedUserId, targetUserId));
}
