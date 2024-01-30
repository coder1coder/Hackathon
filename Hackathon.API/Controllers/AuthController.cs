using System.Net;
using System.Threading.Tasks;
using Hackathon.API.Module;
using Hackathon.Common.Abstraction.Auth;
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
public class AuthController(IMapper mapper, IAuthService authService) : BaseController
{
    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost(nameof(SignIn))]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AuthTokenModel))]
    public Task<IActionResult> SignIn([FromBody] SignInRequest request)
        => GetResult(() =>authService.SignInAsync(mapper.Map<SignInRequest, SignInModel>(request)));

    /// <summary>
    /// Авторизация пользователя через Google
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost(nameof(SignInByGoogle))]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AuthTokenModel))]
    public Task<IActionResult> SignInByGoogle([FromBody] SignInByGoogleModel parameters)
        => GetResult(() => authService.SignInByGoogleAsync(parameters));
}
