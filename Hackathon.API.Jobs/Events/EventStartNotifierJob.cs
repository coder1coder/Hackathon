using Hackathon.Common.Abstraction.Notification;
using Hackathon.Common.Models.Notification;
using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.Events;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hackathon.Jobs.Events;

/// <summary>
/// Уведомляет организатора о начале предстоящего события
/// </summary>
public sealed class EventStartNotifierJob: BaseJob<EventStartNotifierJob>
{
    private readonly IEventService _eventService;
    private readonly INotificationService _notificationService;

    public EventStartNotifierJob(
        IEventService eventService,
        INotificationService notificationService,
        ILogger<EventStartNotifierJob> logger):base(logger)
    {
        _eventService = eventService;
        _notificationService = notificationService;
    }

    protected override async Task DoWork(IJobExecutionContext context)
    {
        var eventsResult = await _eventService.GetUpcomingEventsAsync(TimeSpan.FromMinutes(5));

        if (eventsResult.IsSuccess && eventsResult.Data.Items is {Count: > 0})
            await _notificationService.PushManyAsync(
                eventsResult.Data.Items.Select(x =>
                    NotificationFactory.InfoNotification($"Событие '{x.Name}' скоро начнется", x.OwnerId)));
    }
}
