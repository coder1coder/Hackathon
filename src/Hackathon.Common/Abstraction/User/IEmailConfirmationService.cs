using BackendTools.Common.Models;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.User;

public interface IEmailConfirmationService
{
    /// <summary>
    /// Подтвердить Email
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="code">Код подтверждения</param>
    Task<Result> Confirm(long userId, string code);

    /// <summary>
    /// Создать запрос на подтвреждение Email пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    Task<Result> CreateRequest(long userId);
}
