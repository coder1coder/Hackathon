using System.Net;
using System.Threading.Tasks;
using Hackathon.API.Contracts.Users;
using Hackathon.API.Module;
using Hackathon.Common.Abstraction.Auth;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Users;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

/// <summary>
/// Авторизация и аутентификация
/// </summary>
[SwaggerTag("Авторизация и аутентификация")]
public class AuthController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;

    /// <summary>
    /// Авторизация и аутентификация
    /// </summary>
    public AuthController(IMapper mapper, IAuthService authService)
    {
        _mapper = mapper;
        _authService = authService;
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="request"></param>
    [AllowAnonymous]
    [HttpPost(nameof(SignIn))]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AuthTokenModel))]
    public Task<IActionResult> SignIn([FromBody] SignInRequest request)
        => GetResult(() =>_authService.SignInAsync(_mapper.Map<SignInRequest, SignInModel>(request)));

    /// <summary>
    /// Авторизация пользователя через Google
    /// </summary>
    /// <param name="parameters"></param>
    [AllowAnonymous]
    [HttpPost(nameof(SignInByGoogle))]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AuthTokenModel))]
    public Task<IActionResult> SignInByGoogle([FromBody] SignInByGoogleModel parameters)
        => GetResult(() => _authService.SignInByGoogleAsync(parameters));
}
