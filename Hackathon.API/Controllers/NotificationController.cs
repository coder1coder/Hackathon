using Hackathon.Common.Abstraction.Notification;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Notification;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Hackathon.API.Controllers;

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
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost("read")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public Task MarkAsRead([FromBody] Guid[] ids)
        => _notificationService.MarkAsReadAsync(AuthorizedUserId, ids);

    /// <summary>
    /// Удалить уведомления пользователя
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete("remove")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task Delete([FromBody] Guid[] ids)
    {
        await _notificationService.DeleteAsync(AuthorizedUserId, ids);
    }

    [HttpPost("push/info")]
    public async Task PushInformationNotification([FromBody] string message, [Required] long to)
    {
        await _notificationService.PushAsync(new CreateNotificationModel<InfoNotificationData>
        {
            UserId = to,
            OwnerId = AuthorizedUserId,
            Type = NotificationType.Information,
            Data = new InfoNotificationData
            {
                Message = message
            }
        });
    }

    [HttpGet("unread/count")]
    public Task<long> GetUnreadNotificationsCount()
        => _notificationService.GetUnreadNotificationsCountAsync(AuthorizedUserId);
}
