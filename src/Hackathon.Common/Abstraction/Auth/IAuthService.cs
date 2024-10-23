using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Users;

namespace Hackathon.Common.Abstraction.Auth;

/// <summary>
/// Методы авторизации/аутентификации
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="signInModel"></param>
    Task<Result<AuthTokenModel>> SignInAsync(SignInModel signInModel);

    /// <summary>
    /// Авторизация пользователя через Google
    /// </summary>
    /// <param name="signInByGoogleModel"></param>
    Task<Result<AuthTokenModel>> SignInByGoogleAsync(SignInByGoogleModel signInByGoogleModel);
}
