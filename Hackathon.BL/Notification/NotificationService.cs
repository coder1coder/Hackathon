using System;
using System.Threading.Tasks;
using Hackathon.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Notification;
using Hackathon.Notification;
using Hackathon.Notification.IntegrationEvent;

namespace Hackathon.BL.Notification;

public class NotificationService: INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IMessageHub<NotificationPublishedIntegrationEvent> _notificationsHub;

    public NotificationService(
        INotificationRepository notificationRepository,
        IMessageHub<NotificationPublishedIntegrationEvent> notificationsHub)
    {
        _notificationRepository = notificationRepository;
        _notificationsHub = notificationsHub;
    }

    public async Task<BaseCollectionModel<NotificationModel>> GetList(GetListModel<NotificationFilterModel> listModel, long userId)
    {
        return await _notificationRepository.GetList(listModel, userId);
    }

    /// <inheritdoc cref="INotificationService.MarkAsRead"/>
    public async Task MarkAsRead(long userId, Guid[] ids = null)
    {
        await _notificationRepository.MarkAsRead(userId, ids);
    }

    /// <inheritdoc cref="INotificationService.Delete"/>
    public async Task Delete(long userId, Guid[] ids = null)
    {
        await _notificationRepository.Delete(userId, ids);
    }

    public async Task Push<T>(CreateNotificationModel<T> model) where T: class
    {
        await _notificationRepository.Push(model);
        await _notificationsHub.Publish(TopicNames.NotificationPublished, new NotificationPublishedIntegrationEvent());
    }

    public async Task<long> GetUnreadNotificationsCount(long userId)
    {
        return await _notificationRepository.GetUnreadNotificationsCount(userId);
    }
}