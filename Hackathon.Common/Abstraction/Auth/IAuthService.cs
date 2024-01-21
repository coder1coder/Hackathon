using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Abstraction.Auth;

public interface IAuthService
{
    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="signInModel"></param>
    /// <returns></returns>
    Task<Result<AuthTokenModel>> SignInAsync(SignInModel signInModel);

    /// <summary>
    /// Авторизация пользователя через Google
    /// </summary>
    /// <param name="signInByGoogleModel"></param>
    /// <returns></returns>
    Task<Result<AuthTokenModel>> SignInByGoogleAsync(SignInByGoogleModel signInByGoogleModel);
}
