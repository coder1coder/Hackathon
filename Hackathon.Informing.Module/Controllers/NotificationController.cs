using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hackathon.API.Module;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Informing.Abstractions.Models;
using Hackathon.Informing.Abstractions.Services;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.Informing.Module.Controllers;

[SwaggerTag("Уведомления")]
public class NotificationController: BaseController
{
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public NotificationController(INotificationService notificationService, IMapper mapper)
    {
        _notificationService = notificationService;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить уведомления пользователя по заданным фильтрам и параметрам
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("list")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseCollection<NotificationModel>), StatusCodes.Status200OK)]
    public async Task<BaseCollection<NotificationModel>> GetList([FromBody] GetListParameters<NotificationFilterModel> request)
    {
        var filterModel = _mapper.Map<GetListParameters<NotificationFilterModel>>(request);
        return await _notificationService.GetListAsync(filterModel, AuthorizedUserId);
    }

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
    /// Отправить информационное сообщение
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="recipientId">Идентификатор получателя (пользователя)</param>
    [HttpPost("push/info")]
    public Task PushInformationNotification([FromBody] string message, [Required] long recipientId)
        => _notificationService.PushAsync(new CreateNotificationModel<InfoNotificationData>
        {
            UserId = recipientId,
            OwnerId = AuthorizedUserId,
            Type = NotificationType.System,
            Data = new InfoNotificationData
            {
                Message = message
            }
        });

    /// <summary>
    /// Получить количество непрочитанных уведомлений
    /// </summary>
    /// <returns></returns>
    [HttpGet("unread/count")]
    public Task<int> GetUnreadNotificationsCount()
        => _notificationService.GetUnreadNotificationsCountAsync(AuthorizedUserId);
}
