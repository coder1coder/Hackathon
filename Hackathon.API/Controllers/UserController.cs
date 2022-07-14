using System.Threading.Tasks;
using Hackathon.Abstraction.User;
using Hackathon.API.Abstraction;
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
public class UserController: BaseController, IUserApi
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    /// <summary>
    /// Пользователи
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="userService"></param>
    public UserController(
        IMapper mapper,
        IUserService userService
    )
    {
        _mapper = mapper;
        _userService = userService;
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
    [HttpGet]
    [Route("{userId:long}")]
    public async Task<UserResponse> Get([FromRoute] long userId)
        => _mapper.Map<UserResponse>(await _userService.GetAsync(userId));

    /// <summary>
    /// Получить все реакции на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получившего реакцию</param>
    /// <returns></returns>
    [HttpGet("{userId:long}/reactions")]
    public async Task<UserProfileReaction[]> GetReactions(long userId)
        => await _userService.GetReactionsAsync(UserId, userId);

    /// <summary>
    /// Добавить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получающего реакцию</param>
    /// <param name="reaction">Реакция</param>
    [HttpPost("{userId:long}/reactions/{reaction}")]
    public async Task AddReaction(long userId, UserProfileReaction reaction)
        => await _userService.AddReactionAsync(UserId, userId, reaction);

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId">Идентификатор пользователя получившего реакцию</param>
    /// <param name="reaction">Реакция</param>
    [HttpDelete("{userId:long}/reactions/{reaction}")]
    public async Task RemoveReaction(long userId, UserProfileReaction reaction)
        => await _userService.RemoveReactionAsync(UserId, userId, reaction);

    /// <summary>
    /// Получить информацию о пользователях
    /// </summary>
    /// <returns></returns>
    [HttpPost("list")]
    [Authorize(Policy = nameof(UserRole.Administrator))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseCollectionResponse<UserResponse>))]
    public async Task<IActionResult> GetAsync([FromBody] GetListParameters<UserFilter> request)
    {
        var getFilterModel = _mapper.Map<GetListParameters<UserFilter>>(request);
        var collectionModel = await _userService.GetAsync(getFilterModel);
        return Ok(_mapper.Map<BaseCollectionResponse<UserResponse>>(collectionModel));
    }

    /// <summary>
    /// Загрузить картинку профиля пользователя.
    /// </summary>
    /// <param name="file">Файл картинка.</param>
    /// <returns></returns>
    [HttpPost]
    [Route("profile/image/upload")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UploadProfileImage(IFormFile file)
    {
        if (file is null)
            return BadRequest($"Файл не может быть пустым.");

        await using var stream = file.OpenReadStream();

        var newProfileImageId = await _userService.UploadProfileImageAsync(UserId, file.FileName, stream);

        return Ok(newProfileImageId);
    }
}
