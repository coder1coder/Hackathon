using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Abstraction;
using Hackathon.Abstraction.Entities;
using Hackathon.Abstraction.Jobs;
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
        private readonly IEventRepository _eventRepository;
        private readonly INotificationService _notificationService;

        public EventStartNotifierJob(
            IEventRepository eventRepository, 
            INotificationService notificationService)
        {
            _eventRepository = eventRepository;
            _notificationService = notificationService;
        }

        public async Task Execute()
        {
            var now = DateTime.UtcNow.AddMinutes(5).ToUtcWithoutSeconds();

            var events = await _eventRepository.GetByExpression(x =>
                DateTime.Compare(now, x.Start) == 0 
                && x.Status == EventStatus.Published);
            
            if (events.Any())
                foreach (var ev in events)
                    await _notificationService.Push(NotificationFactory
                        .InfoNotification($"Событие '{ev.Name}' скоро начнется", ev.UserId));
        }
    }
}