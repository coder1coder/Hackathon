using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackathon.Abstraction.IntegrationEvents;
using Hackathon.Abstraction.Notification;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Notification;
using Hackathon.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvent;

namespace Hackathon.BL.Notification;

public class NotificationService: INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IMessageHub<NotificationChangedIntegrationEvent> _notificationsHub;

    public NotificationService(
        INotificationRepository notificationRepository,
        IMessageHub<NotificationChangedIntegrationEvent> notificationsHub)
    {
        _notificationRepository = notificationRepository;
        _notificationsHub = notificationsHub;
    }

    public Task<BaseCollection<NotificationModel>> GetList(GetListParameters<NotificationFilterModel> listParameters, long userId)
        => _notificationRepository.GetList(listParameters, userId);

    public Task MarkAsRead(long userId, Guid[] ids)
        => _notificationRepository.MarkAsRead(userId, ids);

    public async Task Delete(long userId, Guid[] ids = null)
    {
        await _notificationRepository.Delete(userId, ids);
        await _notificationsHub.Publish(TopicNames.NotificationChanged,
            new NotificationChangedIntegrationEvent(NotificationChangedOperation.Deleted, ids));
    }

    public async Task Push<T>(CreateNotificationModel<T> model) where T: class
    {
        var notificationId = await _notificationRepository.Push(model);
        await _notificationsHub.Publish(TopicNames.NotificationChanged,
            new NotificationChangedIntegrationEvent(NotificationChangedOperation.Created, new []{ notificationId }));
    }

    public async Task PushMany<T>(IEnumerable<CreateNotificationModel<T>> models) where T : class
    {
        var ids = new List<Guid>();
        foreach (var model in models)
            ids.Add(await _notificationRepository.Push(model));

        await _notificationsHub.Publish(TopicNames.NotificationChanged,
            new NotificationChangedIntegrationEvent(NotificationChangedOperation.Created, ids.ToArray()));
    }

    public Task<long> GetUnreadNotificationsCount(long userId)
        => _notificationRepository.GetUnreadNotificationsCount(userId);
}
