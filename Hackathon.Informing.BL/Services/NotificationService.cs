using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Informing.Abstractions.IntegrationEvents;
using Hackathon.Informing.Abstractions.Models;
using Hackathon.Informing.Abstractions.Repositories;
using Hackathon.Informing.Abstractions.Services;

namespace Hackathon.Informing.BL.Services;

public class NotificationService: INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IInformingIntegrationEventsHub _informingIntegrationEventsHub;

    public NotificationService(
        INotificationRepository notificationRepository,
        IInformingIntegrationEventsHub informingIntegrationEventsHub)
    {
        _notificationRepository = notificationRepository;
        _informingIntegrationEventsHub = informingIntegrationEventsHub;
    }

    public Task<BaseCollection<NotificationModel>> GetListAsync(GetListParameters<NotificationFilterModel> listParameters, long userId)
        => _notificationRepository.GetList(listParameters, userId);

    public Task MarkAsReadAsync(long userId, Guid[] notificationIds)
        => _notificationRepository.MarkAsRead(userId, notificationIds);

    public async Task DeleteAsync(long userId, Guid[] notificationIds = null)
    {
        await _notificationRepository.Delete(userId, notificationIds);
        await _informingIntegrationEventsHub.PublishAll(new NotificationChangedIntegrationEvent(
            NotificationChangedOperation.Deleted, notificationIds));
    }

    public async Task PushAsync<T>(CreateNotificationModel<T> model) where T: class
    {
        var notificationIds = await _notificationRepository.AddManyAsync(new []{ model });
        await _informingIntegrationEventsHub.PublishAll(new NotificationChangedIntegrationEvent(
            NotificationChangedOperation.Created, notificationIds));
    }

    public async Task PushManyAsync<T>(IEnumerable<CreateNotificationModel<T>> models) where T : class
    {
        var notificationIds = await _notificationRepository.AddManyAsync(models.ToArray());
        await _informingIntegrationEventsHub.PublishAll(new NotificationChangedIntegrationEvent(
            NotificationChangedOperation.Created, notificationIds));
    }

    public Task<int> GetUnreadNotificationsCountAsync(long userId)
        => _notificationRepository.GetUnreadNotificationsCount(userId);
}
