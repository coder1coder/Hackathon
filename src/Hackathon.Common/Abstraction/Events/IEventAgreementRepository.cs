using System.Threading.Tasks;
using Hackathon.Common.Models.Event;

namespace Hackathon.Common.Abstraction.Events;

/// <summary>
/// Провайдер данных соглашений мероприятий
/// </summary>
public interface IEventAgreementRepository
{
    /// <summary>
    /// Получить <see cref="EventAgreementModel">соглашение об участии в мероприятии</see> по идентификатору мероприятия
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    Task<EventAgreementModel> GetByEventId(long eventId);

    /// <summary>
    /// Добавить/обновить связь соглашения с пользователем
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <param name="userId">Идентификатор пользователя</param>
    Task UpsertUserRelationAsync(long eventId, long userId);
}
