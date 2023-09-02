using System;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.Event;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.User;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hackathon.Jobs.Events;

public class StartedEventStatusUpdateJob: BaseJob<StartedEventStatusUpdateJob>
{
    private readonly IEventService _eventService;
    private readonly ILogger<StartedEventStatusUpdateJob> _logger;

    public StartedEventStatusUpdateJob(
        IEventService eventService,
        ILogger<StartedEventStatusUpdateJob> logger):base(logger)
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
                StartFrom = DateTime.UtcNow,
                Statuses = new[] { EventStatus.Published }
            },
            Offset = 0,
            Limit = 2_000
        });

        if (!result.IsSuccess)
            return;

        if (result.Data.Items is not { Count: > 0 })
        {
            _logger.LogInformation("{Source}: нет мероприятий для перевода в статус <Начато>",
                nameof(StartedEventStatusUpdateJob));
            return;
        }

        var changesCount = 0;
        foreach (var eventListItem in result.Data.Items)
        {
            try
            {
                var setStatusResult = await _eventService.SetStatusAsync(default, eventListItem.Id,
                    EventStatus.Started,
                    skipUserValidation: true,
                    skipUserValidationRole: UserRole.Administrator,
                    skipValidation: false);

                if (!setStatusResult.IsSuccess)
                {
                    _logger.LogWarning("{Source} Не удалось обновить статус мероприятия в <Начато>: {@Reason}",
                        nameof(StartedEventStatusUpdateJob),
                        string.Join(", ", setStatusResult.Errors.Values.Values));
                    continue;
                }

                changesCount++;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Source}: ошибка во время обновления статуса мероприятия",
                    nameof(StartedEventStatusUpdateJob));
            }
        }

        _logger.LogInformation("{Source}: {EventsCount} мероприятий переведны в статус <Начато>",
            nameof(StartedEventStatusUpdateJob),
            changesCount);
    }
}
