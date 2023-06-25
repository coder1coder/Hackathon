using Hackathon.Common.Abstraction.Event;
using Hackathon.Common.Abstraction.Notification;
using Hackathon.Common.Models.Notification;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.Jobs.Events;

/// <summary>
/// Уведомляет организатора о начале предстоящего события
/// </summary>
public class EventStartNotifierJob: IJob
{
    private readonly IEventService _eventService;
    private readonly INotificationService _notificationService;

    public EventStartNotifierJob(
        IEventService eventService,
        INotificationService notificationService)
    {
        _eventService = eventService;
        _notificationService = notificationService;
    }

    public async Task ExecuteAsync()
    {
        var eventsResult = await _eventService.GetUpcomingEventsAsync(TimeSpan.FromMinutes(5));

        if (eventsResult.IsSuccess && eventsResult.Data.Items is {Count: > 0})
            await _notificationService.PushManyAsync(
                eventsResult.Data.Items.Select(x =>
                    NotificationFactory.EventWillStartSoon(x.OwnerId, x.Name)));
    }
}
