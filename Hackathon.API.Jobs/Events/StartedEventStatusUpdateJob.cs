using System;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.Event;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Microsoft.Extensions.Logging;

namespace Hackathon.Jobs.Events;

public class StartedEventStatusUpdateJob: IJob
{
    private readonly IEventService _eventService;
    private readonly ILogger<StartedEventStatusUpdateJob> _logger;

    public StartedEventStatusUpdateJob(
        IEventService eventService,
        ILogger<StartedEventStatusUpdateJob> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        var result = await _eventService.GetListAsync(default, new GetListParameters<EventFilter>
        {
            Filter = new EventFilter
            {
                StartFrom = DateTime.UtcNow,
                Statuses = new[] { EventStatus.Published }
            },
            Offset = 0,
            Limit = 2_000
        });

        if (!result.IsSuccess)
            return;

        if (result.Data.Items is {Count: > 0})
        {
            var changesCount = 0;
            foreach (var eventListItem in result.Data.Items)
            {
                try
                {
                    var setStatusResult = await _eventService.SetStatusAsync(eventListItem.Id, EventStatus.Started);
                    if (!setStatusResult.IsSuccess)
                    {
                        changesCount++;
                    }
                    else
                    {
                        _logger.LogWarning("{Source} Не удалось обновить статус мероприятия в <Начато>: {@Reason}",
                            nameof(StartedEventStatusUpdateJob),
                            string.Join(", ", setStatusResult.Errors.Values.Values));
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e,
                        "{Source}: ошибка во время обновления статуса мероприятия",
                        nameof(StartedEventStatusUpdateJob));
                }
            }

            _logger.LogInformation("{Source}: {EventsCount} мероприятий переведны в статус <Начато>",
                nameof(StartedEventStatusUpdateJob),
                changesCount);
        }
        else
        {
            _logger.LogInformation("{Source}: нет мероприятий для перевода в статус <Начато>",
                nameof(StartedEventStatusUpdateJob));
        }
    }
}
