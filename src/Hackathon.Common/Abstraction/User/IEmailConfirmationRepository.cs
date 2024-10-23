using System.Threading.Tasks;
using Hackathon.Common.Models.Users;

namespace Hackathon.Common.Abstraction.User;

public interface IEmailConfirmationRepository
{
    /// <summary>
    /// Получить запрос на подтверждение Email по идентификатору пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    Task<EmailConfirmationRequestModel> GetByUserIdAsync(long userId);

    /// <summary>
    /// Добавить запись запроса на подтверждение Email
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    Task AddAsync(EmailConfirmationRequestParameters parameters);

    /// <summary>
    /// Обновить запись запроса на подтверждение Email
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    Task UpdateAsync(EmailConfirmationRequestParameters parameters);

    /// <summary>
    /// Удалить запись запроса на подтверждение Email
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    Task DeleteAsync(long userId);
}
