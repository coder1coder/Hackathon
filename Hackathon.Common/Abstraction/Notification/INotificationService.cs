using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Notification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Notification;

public interface INotificationService
{
    /// <summary>
    /// Получить список уведомлений пользователя по заданным параметрам фильтрации и пагинации
    /// </summary>
    /// <param name="listParameters">Модель фильтра и пагинации</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<BaseCollection<NotificationModel>> GetListAsync(GetListParameters<NotificationFilterModel> listParameters, long userId);

    Task MarkAsReadAsync(long userId, Guid[] ids);

    Task DeleteAsync(long userId, Guid[] ids = null);

    /// <summary>
    /// Отправить уведомление
    /// </summary>
    /// <param name="model"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task PushAsync<T>(CreateNotificationModel<T> model) where T : class;

    /// <summary>
    /// Отправить несколько уведомлений
    /// </summary>
    /// <param name="models"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task PushManyAsync<T>(IEnumerable<CreateNotificationModel<T>> models) where T : class;

    /// <summary>
    /// Получить количество непрочитанных уведомлений
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<long> GetUnreadNotificationsCountAsync(long userId);
}
