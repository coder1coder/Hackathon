using System.Linq.Expressions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Hackathon.Entities;

namespace Hackathon.Abstraction.Event
{
    public interface IEventService
    {
        /// <summary>
        /// Создание события
        /// </summary>
        /// <param name="createEventModel">Модель создаваемого события</param>
        /// <returns></returns>
        Task<long> CreateAsync(CreateEventModel createEventModel);

        /// <summary>
        /// Редактирование события
        /// </summary>
        /// <param name="updateEventModel"></param>
        /// <returns></returns>
        Task UpdateAsync(UpdateEventModel updateEventModel);

        /// <summary>
        /// Получение информации о событии по идентификатору
        /// </summary>
        /// <param name="eventId">Идентификатор события</param>
        /// <returns></returns>
        Task<EventModel> GetAsync(long eventId);

        /// <summary>
        /// Получение информации о событиях
        /// </summary>
        /// <param name="userId">Идентификатор авторизованного пользователя</param>
        /// <param name="getListParameters">Фильтр, пагинация</param>
        /// <returns></returns>
        Task<BaseCollection<EventListItem>> GetListAsync(long userId, GetListParameters<EventFilter> getListParameters);

        /// <summary>
        /// Изменение статуса события
        /// </summary>
        /// <param name="eventId">Идентификатор обытия</param>
        /// <param name="eventStatus">Новый статус события</param>
        /// <returns></returns>
        Task SetStatusAsync(long eventId, EventStatus eventStatus);

        /// <summary>
        /// Добавление пользователя к событию
        /// </summary>
        /// <param name="eventId">Идентификатор события</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        Task JoinAsync(long eventId, long userId);

        /// <summary>
        /// Покинуть событие
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task LeaveAsync(long eventId, long userId);

        /// <summary>
        /// Удаление события
        /// </summary>
        /// <param name="eventId">Идентификатор события</param>
        /// <returns></returns>
        Task DeleteAsync(long eventId);
        
        /// <summary>
        /// Получить события по выражению
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<EventModel[]> GetByExpression(Expression<Func<EventEntity, bool>> expression);
    }
}
