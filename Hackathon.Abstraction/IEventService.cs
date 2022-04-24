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
        /// <param name="getListModel">Фильтр, пагинация</param>
        /// <returns></returns>
        Task<BaseCollectionModel<EventModel>> GetAsync(long userId, GetListModel<EventFilterModel> getListModel);

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

        Task JoinTeamAsync(long eventId, long teamId, long userId);

        /// <summary>
        /// Покинуть событие
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task LeaveAsync(long eventId, long userId);
        
        /// <summary>
        /// Покинуть событие командой
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task LeaveTeamAsync(long eventId, long teamId);

        /// <summary>
        /// Удаление события
        /// </summary>
        /// <param name="eventId">Идентификатор события</param>
        /// <returns></returns>
        Task DeleteAsync(long eventId);
    }
}
