using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.Events;
using Quartz;

namespace Hackathon.Jobs.Events;

/// <summary>
/// Переводит в статус "Завершено" мероприятия, дата окончания которых меньше текущей
/// </summary>
public sealed class PastEventStatusUpdateJob: BaseJob<PastEventStatusUpdateJob>
{
    private readonly IEventService _eventService;
    private readonly ILogger<PastEventStatusUpdateJob> _logger;

    public PastEventStatusUpdateJob(
        IEventService eventService,
        ILogger<PastEventStatusUpdateJob> logger):base(logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    protected override async Task DoWork(IJobExecutionContext context)
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
                    await _eventService.SetStatusAsync(default, eventListItem.Id, EventStatus.Finished, skipValidation: true, skipUserValidation: true);
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
