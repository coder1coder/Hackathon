using System.Threading.Tasks;
using Hackathon.Common.Models.Event;

namespace Hackathon.Common.Abstraction.Event;

public interface IEventAgreementRepository
{
    /// <summary>
    /// Получить <see cref="EventAgreementModel">соглашение об участии в мероприятии</see> по идентификатору мероприятия
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <returns></returns>
    Task<EventAgreementModel> GetByEventId(long eventId);

    /// <summary>
    /// Добавить/обновить связь соглашения с пользователем
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task UpsertUserRelationAsync(long eventId, long userId);
}
