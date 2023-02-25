using System.Linq.Expressions;
using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Hackathon.Entities;
using Hackathon.Entities.Event;

namespace Hackathon.Abstraction.Event;

public interface IEventService
{
    /// <summary>
    /// Создание события
    /// </summary>
    /// <param name="eventCreateParameters">Модель создаваемого события</param>
    /// <returns></returns>
    Task<Result<long>> CreateAsync(EventCreateParameters eventCreateParameters);

    /// <summary>
    /// Редактирование события
    /// </summary>
    /// <param name="eventUpdateParameters"></param>
    /// <returns></returns>
    Task<Result> UpdateAsync(EventUpdateParameters eventUpdateParameters);

    /// <summary>
    /// Получение информации о событии по идентификатору
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <returns></returns>
    Task<Result<EventModel>> GetAsync(long eventId);

    /// <summary>
    /// Получение информации о событиях
    /// </summary>
    /// <param name="userId">Идентификатор авторизованного пользователя</param>
    /// <param name="getListParameters">Фильтр, пагинация</param>
    /// <returns></returns>
    Task<Result<BaseCollection<EventListItem>>> GetListAsync(long userId, Common.Models.GetListParameters<EventFilter> getListParameters);

    /// <summary>
    /// Изменение статуса события
    /// </summary>
    /// <param name="eventId">Идентификатор обытия</param>
    /// <param name="eventStatus">Новый статус события</param>
    /// <returns></returns>
    Task<Result> SetStatusAsync(long eventId, EventStatus eventStatus);

    /// <summary>
    /// Добавление пользователя к событию
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<Result> JoinAsync(long eventId, long userId);

    /// <summary>
    /// Покинуть событие
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Result> LeaveAsync(long eventId, long userId);

    /// <summary>
    /// Удаление события
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <returns></returns>
    Task<Result> DeleteAsync(long eventId);

    /// <summary>
    /// Получить события по выражению
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<Result<EventModel[]>> GetByExpression(Expression<Func<EventEntity, bool>> expression);
}
