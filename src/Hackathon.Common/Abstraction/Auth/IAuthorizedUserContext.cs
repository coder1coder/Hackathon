using Hackathon.Common.Models.Auth;

namespace Hackathon.Common.Abstraction.Auth;

/// <summary>
/// Контекст авторизованного пользователя
/// </summary>
public interface IAuthorizedUserContext
{
    /// <summary>
    /// Получить авторизованного пользователя
    /// </summary>
    AuthorizedUser GetAuthorizedUser();
}
