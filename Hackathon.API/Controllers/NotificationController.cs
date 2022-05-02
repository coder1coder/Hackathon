using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hackathon.Abstraction;
using Hackathon.Abstraction.Notification;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Notification;
using Hackathon.Contracts.Requests;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

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
    [ProducesResponseType(typeof(BaseCollectionResponse<NotificationModel>), StatusCodes.Status200OK)]
    public async Task<BaseCollectionResponse<NotificationModel>> GetList(GetListRequest<NotificationFilterModel> request)
    {
        var filterModel = _mapper.Map<GetListModel<NotificationFilterModel>>(request);
        var collectionModel = await _notificationService.GetList(filterModel, UserId);
        return _mapper.Map<BaseCollectionResponse<NotificationModel>>(collectionModel);
    }

    /// <summary>
    /// Отметить уведомления пользователя как прочтенные
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost("read")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task MarkAsRead([FromBody] Guid[] ids)
    {
        await _notificationService.MarkAsRead(UserId, ids);
    }

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
        await _notificationService.Delete(UserId, ids);
    }

    [HttpPost("push/info")]
    public async Task PushInformationNotification([FromBody] string message, [Required] long to)
    {
        await _notificationService.Push(new CreateNotificationModel<InfoNotificationData>()
        {
            UserId = to,
            OwnerId = UserId,
            Type = NotificationType.Information,
            Data = new InfoNotificationData
            {
                Message = message
            }
        });
    }

    [HttpGet("unread/count")]
    public async Task<long> GetUnreadNotificationsCount()
    {
        return await _notificationService.GetUnreadNotificationsCount(UserId);
    }
}