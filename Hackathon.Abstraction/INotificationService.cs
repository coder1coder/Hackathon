using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Notification;

namespace Hackathon.Abstraction;

public interface INotificationService
{
    /// <summary>
    /// Получить список уведомлений пользователя по заданным параметрам фильтрации и пагинации
    /// </summary>
    /// <param name="listModel">Модель фильтра и пагинации</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<BaseCollectionModel<NotificationModel>> GetList(GetListModel<NotificationFilterModel> listModel, long userId);

    /// <inheritdoc cref="INotificationRepository.MarkAsRead(long, Guid[])"/>
    Task MarkAsRead(long userId, Guid[]? ids = null);

    /// <inheritdoc cref="INotificationRepository.Delete(long, Guid[])"/>
    Task Delete(long userId, Guid[]? ids = null);

    Task Push<T>(CreateNotificationModel<T> model) where T : class;

    Task<long> GetUnreadNotificationsCount(long userId);
}