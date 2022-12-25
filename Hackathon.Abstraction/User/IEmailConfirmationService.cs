using BackendTools.Common.Models;

namespace Hackathon.Abstraction.User;

public interface IEmailConfirmationService
{
    /// <summary>
    /// Подтвердить Email
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="code">Код подтверждения</param>
    /// <returns></returns>
    Task<Result> Confirm(long userId, string code);

    /// <summary>
    /// Создать запрос на подтвреждение Email пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<Result> CreateRequest(long userId);
}
