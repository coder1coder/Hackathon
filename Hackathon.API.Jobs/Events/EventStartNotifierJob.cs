using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Informing.Abstractions.Models.Notifications.Data;
using Hackathon.Informing.Abstractions.Services;
using Hackathon.Informing.BL;
using Microsoft.Extensions.Logging;

namespace Hackathon.Jobs.Events;

/// <summary>
/// Уведомляет организатора о начале предстоящего события
/// </summary>
public sealed class EventStartNotifierJob: BaseBackgroundJob<EventStartNotifierJob>
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

    public override async Task DoWork()
    {
        var eventsResult = await _eventService.GetUpcomingEventsAsync(TimeSpan.FromMinutes(5));

        if (eventsResult.IsSuccess && eventsResult.Data.Items is {Count: > 0})
            await _notificationService.PushManyAsync(
                eventsResult.Data.Items.Select(x =>
                    NotificationCreator.System(new SystemNotificationData($"Событие '{x.Name}' скоро начнется"),
                        x.OwnerId)));
    }
}
