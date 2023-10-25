using Hackathon.Common.Abstraction.User;
using System.Net;
using System.Threading.Tasks;
using Hackathon.API.Module;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.User;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

/// <summary>
/// Авторизация и аутентификация
/// </summary>
[SwaggerTag("Авторизация и аутентификация")]
public class AuthController: BaseController
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public AuthController(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost(nameof(SignIn))]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AuthTokenModel))]
    public Task<IActionResult> SignIn([FromBody] SignInRequest request)
        => GetResult(() =>_userService.SignInAsync(_mapper.Map<SignInModel>(request)));

    /// <summary>
    /// Авторизация пользователя через Google
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost(nameof(SignInByGoogle))]
    public async Task<AuthTokenModel> SignInByGoogle([FromBody] SignInGoogleRequest request)
    {
        var signInByGoogleModel = _mapper.Map<SignInByGoogleModel>(request);
        return await _userService.SignInByGoogle(signInByGoogleModel);
    }
}
