using Hackathon.Common.Abstraction.Event;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Hackathon.Jobs.Events;

/// <summary>
/// Переводит в статус "Завершено" мероприятия, дата окончания которых меньше текущей
/// </summary>
public class PastEventStatusUpdateJob: IJob
{
    private readonly IEventService _eventService;
    private readonly ILogger<PastEventStatusUpdateJob> _logger;

    public PastEventStatusUpdateJob(
        IEventService eventService,
        ILogger<PastEventStatusUpdateJob> logger)
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
                StartTo = DateTime.UtcNow,
                Statuses = new[] { EventStatus.Published, EventStatus.Started }
            },
            Offset = 0,
            Limit = 2_000
        });

        if (!result.IsSuccess)
            return;

        if (result.Data.Items is {Count: > 0})
        {
            var processed = 0;
            foreach (var eventListItem in result.Data.Items)
            {
                try
                {
                    await _eventService.SetStatusAsync(eventListItem.Id, EventStatus.Finished, skipValidation: true);
                    processed++;
                }
                catch (Exception e)
                {
                    _logger.LogError(e,
                        "{Source}: ошибка во время обновления статуса мероприятия",
                        nameof(PastEventStatusUpdateJob));
                }
            }

            _logger.LogInformation("{Source}: {EventsCount} мероприятий переведны в статус <Завершено>",
                nameof(PastEventStatusUpdateJob),
                processed);
        }
        else
        {
            _logger.LogInformation("{Source}: нет мероприятий для перевода в статус <Завершено>",
                nameof(PastEventStatusUpdateJob));
        }

    }
}