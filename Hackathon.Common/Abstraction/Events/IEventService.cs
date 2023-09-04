using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using System;
using System.Threading.Tasks;
using Hackathon.Common.Models.User;
using Microsoft.AspNetCore.Http;

namespace Hackathon.Common.Abstraction.Events;

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
    /// <param name="authorizedUserId">Идентификатор авторизованного пользователя</param>
    /// <param name="eventUpdateParameters"></param>
    /// <returns></returns>
    Task<Result> UpdateAsync(long authorizedUserId, EventUpdateParameters eventUpdateParameters);

    /// <summary>
    /// Получение информации о событии по идентификатору
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <returns></returns>
    Task<Result<EventModel>> GetAsync(long eventId);

    /// <summary>
    /// Получение информации о событиях
    /// </summary>
    /// <param name="authorizedUserId">Идентификатор авторизованного пользователя</param>
    /// <param name="parameters">Фильтр, пагинация</param>
    /// <returns></returns>
    Task<Result<BaseCollection<EventListItem>>> GetListAsync(long authorizedUserId, Common.Models.GetListParameters<EventFilter> parameters);

    /// <summary>
    /// Изменение статуса события
    /// </summary>
    /// <param name="authorizedUserId">Идентификатор авторизованного пользователя</param>
    /// <param name="eventId">Идентификатор обытия</param>
    /// <param name="eventStatus">Новый статус события</param>
    /// <param name="skipValidation">Пропустить валидацию (Использовать только в служебных целях)</param>
    /// <param name="skipUserValidation">Пропустить валидацию пользователя (Использовать только в служебных целях)</param>
    /// <param name="skipUserValidationRole">Роль пользователя которая будет использоваться без валидации пользователя</param>
    /// <returns></returns>
    Task<Result> SetStatusAsync(long authorizedUserId, long eventId, EventStatus eventStatus,
        bool skipValidation = false, bool skipUserValidation = false,
        UserRole skipUserValidationRole = UserRole.Default);

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
    /// <param name="eventId">Идентификатор обытия</param>
    /// <param name="authorizedUserId">Идентификатор авторизованного пользователя</param>
    /// <returns></returns>
    Task<Result> LeaveAsync(long eventId, long authorizedUserId);

    /// <summary>
    /// Удаление события
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<Result> DeleteAsync(long eventId, long userId);

    /// <summary>
    /// Получить предстоящие события
    /// </summary>
    /// <returns></returns>
    Task<Result<BaseCollection<EventListItem>>> GetUpcomingEventsAsync(TimeSpan timeBeforeStart);

    /// <summary>
    /// Переключить событие на следующий этап
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="eventId">Идентификатор события</param>
    /// <returns></returns>
    Task<Result> GoNextStage(long userId, long eventId);

    /// <summary>
    /// Загрузить изображение события
    /// </summary>
    /// <param name="file">Файл http запроса</param>
    /// <returns></returns>
    Task<Result<Guid>> UploadEventImageAsync(IFormFile file);

    /// <summary>
    /// Получить соглашение мероприятия
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <returns></returns>
    Task<Result<EventAgreementModel>> GetAgreementAsync(long eventId);

    /// <summary>
    /// Принять соглашение мероприятия
    /// </summary>
    /// <param name="authorizedUserId">Идентификатор пользователя</param>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <returns></returns>
    Task<Result> AcceptAgreementAsync(long authorizedUserId, long eventId);
}
