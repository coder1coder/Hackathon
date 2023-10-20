using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;

namespace Hackathon.Common.Abstraction.Events;

public interface IEventRepository
{
    /// <summary>
    /// Создание события
    /// </summary>
    /// <param name="eventCreateUpdateParameters">Модель создаваемого события</param>
    /// <returns></returns>
    Task<long> CreateAsync(EventCreateParameters eventCreateUpdateParameters);

    /// <summary>
    /// Получение информации о событии по идентификатору
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <returns></returns>
    Task<EventModel> GetAsync(long eventId);

    /// <summary>
    /// Получение список мероприятий
    /// </summary>
    /// <param name="userId">Идентификатор авторизованного пользователя</param>
    /// <param name="parameters">Параметры фильтрации и пагинации</param>
    /// <returns></returns>
    Task<BaseCollection<EventListItem>> GetListAsync(long userId, GetListParameters<EventFilter> parameters);

    /// <summary>
    /// Обновление события
    /// </summary>
    /// <param name="eventUpdateParameters">Событие</param>
    /// <returns></returns>
    Task UpdateAsync(EventUpdateParameters eventUpdateParameters);

    /// <summary>
    /// Изменение статуса события
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="eventStatus">Новый статус события</param>
    /// <returns></returns>
    Task SetStatusAsync(long eventId, EventStatus eventStatus);

    /// <summary>
    /// Удаление события
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <returns></returns>
    Task DeleteAsync(long eventId);

    /// <summary>
    /// Установить идентификатор текущего этапа события
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="stageId">Идентификатор этапа события</param>
    /// <returns></returns>
    Task SetCurrentStageId(long eventId, long stageId);

    /// <summary>
    /// Получить мероприятия по идентификатору временной команды
    /// </summary>
    /// <param name="temporaryTeamId">Идентификатор временной команды</param>
    /// <returns></returns>
    Task<EventModel> GetByTemporaryTeamIdAsync(long temporaryTeamId);
}
