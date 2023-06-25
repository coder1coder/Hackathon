using Hackathon.Common.Abstraction.Notification;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace Hackathon.API.Controllers;

[SwaggerTag("Уведомления")]
public class NotificationController: BaseController
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// Получить уведомления пользователя по заданным фильтрам и параметрам
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("list")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseCollection<NotificationModel>), StatusCodes.Status200OK)]
    public Task<BaseCollection<NotificationModel>> GetList([FromBody] GetListParameters<NotificationFilterModel> request)
        => _notificationService.GetListAsync(request, AuthorizedUserId);

    /// <summary>
    /// Отметить уведомления пользователя как прочтенные
    /// </summary>
    /// <param name="notificationIds">Идентификаторы уведомлений</param>
    /// <returns></returns>
    [HttpPost("read")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public Task MarkAsRead([FromBody] Guid[] notificationIds)
        => _notificationService.MarkAsReadAsync(AuthorizedUserId, notificationIds);

    /// <summary>
    /// Удалить уведомления пользователя
    /// </summary>
    /// <param name="notificationIds">Идентификаторы уведомлений</param>
    /// <returns></returns>
    [HttpDelete("remove")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public Task Delete([FromBody] Guid[] notificationIds)
        => _notificationService.DeleteAsync(AuthorizedUserId, notificationIds);

    /// <summary>
    /// Получить количество непрочитанных уведомлений
    /// </summary>
    /// <returns></returns>
    [HttpGet("unread/count")]
    public Task<long> GetUnreadNotificationsCount()
        => _notificationService.GetUnreadNotificationsCountAsync(AuthorizedUserId);
}
