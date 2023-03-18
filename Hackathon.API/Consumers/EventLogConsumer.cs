using Hackathon.Common.Abstraction.EventLog;
using System;
using System.Threading.Tasks;
using Hackathon.Common.Models.EventLog;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Hackathon.API.Consumers;

public class EventLogConsumer: IConsumer<EventLogModel>
{
    private readonly IEventLogHandler _logHandler;
    private readonly ILogger<EventLogConsumer> _logger;

    public EventLogConsumer(IEventLogHandler logHandler, ILogger<EventLogConsumer> logger)
    {
        _logHandler = logHandler;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EventLogModel> context)
    {
        try
        {
            await _logHandler.Handle(context.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Initiator}. Произошла ошибка при обработке события аудита",
                nameof(EventLogConsumer));
        }
    }
}

