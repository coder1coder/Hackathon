using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hangfire;
using Serilog;

namespace Hackathon.Jobs
{
    public class ChangeEventStatusJob: IChangeEventStatusJob
    {
        private readonly IEventService _eventService;

        public ChangeEventStatusJob(
            IEventService eventService
            )
        {
            Name = nameof(ChangeEventStatusJob);
            CronInterval = Cron.Minutely();

            _eventService = eventService;
        }

        public string Name { get; }
        public string CronInterval { get; }

        public async Task Execute()
        {
            var publishedEvents = await _eventService.GetAsync(new GetFilterModel<EventFilterModel>
            {
                Filter = new EventFilterModel
                {
                    Status = EventStatus.Published
                }
            });

            if (publishedEvents.TotalCount > 0)
            {
                var memberRegistrationEndedEvents = publishedEvents.Items
                    .Where(x => x.Start.AddMinutes(x.MemberRegistrationMinutes) >= DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc))
                    .ToImmutableList();

                foreach (var eventModel in memberRegistrationEndedEvents)
                {
                    await _eventService.SetStatusAsync(eventModel.Id, EventStatus.Started);
                    Log.Logger.Information($"{nameof(ChangeEventStatusJob)} EventId: {eventModel.Id}, OldStatus: {eventModel.Status}, NewStatus: {EventStatus.Started}");
                }
            }
        }
    }
}