using Hackathon.Common.Models.User;

namespace Hackathon.Abstraction.User;

public interface IEmailConfirmationRepository
{
    /// <summary>
    /// Получить запрос на подтверждение Email по идентификатору пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<EmailConfirmationRequestModel> GetByUserIdAsync(long userId);

    /// <summary>
    /// Добавить запись запроса на подтверждение Email
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <returns></returns>
    Task AddAsync(EmailConfirmationRequestParameters parameters);

    /// <summary>
    /// Обновить запись запроса на подтверждение Email
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <returns></returns>
    Task UpdateAsync(EmailConfirmationRequestParameters parameters);
}
