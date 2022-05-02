using System.Linq.Expressions;
using Hackathon.Abstraction.Entities;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;

namespace Hackathon.Abstraction.Event
{
    public interface IEventRepository
    {
        /// <summary>
        /// Создание события
        /// </summary>
        /// <param name="createUpdateEventModel">Модель создаваемого события</param>
        /// <returns></returns>
        Task<long> CreateAsync(CreateEventModel createUpdateEventModel);

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
        /// <param name="getListModel">Фильтр, пагинация</param>
        /// <returns></returns>
        Task<BaseCollectionModel<EventModel>> GetAsync(long userId, GetListModel<EventFilterModel> getListModel);

        /// <summary>
        /// As queryable
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<EventModel[]> GetByExpression(Expression<Func<EventEntity, bool>> expression);

        /// <summary>
        /// Обновление события
        /// </summary>
        /// <param name="updateEventModel">Событие</param>
        /// <returns></returns>
        Task UpdateAsync(UpdateEventModel updateEventModel);

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
    }
}
