using System;
using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Informing.Abstractions.Models;

namespace Hackathon.Informing.Abstractions.Repositories;

public interface INotificationRepository
{
    /// <summary>
    /// Сохранить уведомления
    /// </summary>
    /// <param name="notifications">Уведомления</param>
    /// <returns></returns>
    Task<Guid[]> PushManyAsync<T>(CreateNotificationModel<T>[] notifications) where T : class;

    /// <summary>
    /// Получить список уведомлений пользователя по заданным параметрам фильтрации и пагинации
    /// </summary>
    /// <param name="getListParameters">Модель фильтра и пагинации</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<BaseCollection<NotificationModel>> GetList(GetListParameters<NotificationFilterModel> getListParameters, long userId);

    /// <summary>
    /// Отметить уведомления пользователя как прочтенные
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task MarkAsRead(long userId, Guid[] ids);

    /// <summary>
    /// Отметить уведомления пользователя как удаленные
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task Delete(long userId, Guid[] ids = null);

    Task<long> GetUnreadNotificationsCount(long userId);
}
