﻿using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;

namespace Hackathon.Abstraction.Event;

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
    /// Получение информации о событиях
    /// </summary>
    /// <param name="userId">Идентификатор авторизованного пользователя</param>
    /// <param name="parameters">Фильтр, пагинация</param>
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
    /// Проверяет, существует ли событие
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <returns></returns>
    Task<bool> ExistsAsync(long eventId);
}
