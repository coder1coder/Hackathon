using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Abstraction.Event;
using Hackathon.Abstraction.Jobs;
using Hackathon.Abstraction.Notification;
using Hackathon.Common.Extensions;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Notification;

namespace Hackathon.Jobs.Services
{
    /// <summary>
    /// Уведомляет организатора о начале предстоящего события
    /// </summary>
    public class EventStartNotifierJob: IEventStartNotifierJob
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

        public async Task Execute()
        {
            var now = DateTime.UtcNow.AddMinutes(5).ToUtcWithoutSeconds();

            var events = await _eventService.GetByExpression(x =>
                DateTime.Compare(now, x.Start) == 0 
                && x.Status == EventStatus.Published);

            if (events.Any())
                await _notificationService.PushMany(
                    events.Select(x => NotificationFactory
                    .InfoNotification($"Событие '{x.Name}' скоро начнется", x.OwnerId)));
        }
    }
}