using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Notification;

namespace Hackathon.Abstraction.Notification;

public interface INotificationService
{
    /// <summary>
    /// Получить список уведомлений пользователя по заданным параметрам фильтрации и пагинации
    /// </summary>
    /// <param name="listParameters">Модель фильтра и пагинации</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<BaseCollection<NotificationModel>> GetList(GetListParameters<NotificationFilterModel> listParameters, long userId);

    /// <inheritdoc cref="INotificationRepository.MarkAsRead"/>
    Task MarkAsRead(long userId, Guid[] ids);

    /// <inheritdoc cref="INotificationRepository.Delete(long, Guid[])"/>
    Task Delete(long userId, Guid[]? ids = null);

    /// <summary>
    /// Отправить уведомление
    /// </summary>
    /// <param name="model"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task Push<T>(CreateNotificationModel<T> model) where T : class;
    
    /// <summary>
    /// Отправить несколько уведомлений
    /// </summary>
    /// <param name="models"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task PushMany<T>(IEnumerable<CreateNotificationModel<T>> models) where T : class;

    /// <summary>
    /// Получить количество непрочитанных уведомлений
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<long> GetUnreadNotificationsCount(long userId);
}