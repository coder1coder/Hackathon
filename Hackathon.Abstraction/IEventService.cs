using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;

namespace Hackathon.Abstraction
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
        /// Получение информации о событии по идентификатору
        /// </summary>
        /// <param name="eventId">Идентификатор события</param>
        /// <returns></returns>
        Task<EventModel> GetAsync(long eventId);

        /// <summary>
        /// Получение информации о событиях
        /// </summary>
        /// <param name="getListModel">Фильтр, пагинация</param>
        /// <returns></returns>
        Task<BaseCollectionModel<EventModel>> GetAsync(GetListModel<EventFilterModel> getListModel);

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
        /// Удаление события
        /// </summary>
        /// <param name="eventId">Идентификатор события</param>
        /// <returns></returns>
        Task DeleteAsync(long eventId);
    }
}