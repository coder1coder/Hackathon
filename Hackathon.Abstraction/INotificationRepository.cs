using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Notification;

namespace Hackathon.Abstraction;

public interface INotificationRepository
{
    /// <summary>
    /// Создать новое уведомление в БД
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<Guid> Push<T>(CreateNotificationModel<T> model) where T : class;

    /// <summary>
    /// Получить список уведомлений пользователя по заданным параметрам фильтрации и пагинации
    /// </summary>
    /// <param name="getListModel">Модель фильтра и пагинации</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<BaseCollectionModel<NotificationModel>> GetList(GetListModel<NotificationFilterModel> getListModel, long userId);

    /// <summary>
    /// Отметить уведомления пользователя как прочтенные
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task MarkAsRead(long userId, Guid[]? ids = null);

    /// <summary>
    /// Отметить уведомления пользователя как удаленные
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task Delete(long userId, Guid[]? ids = null);

    Task<long> GetUnreadNotificationsCount(long userId);
}