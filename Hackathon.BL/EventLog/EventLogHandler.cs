using System;
using System.Threading.Tasks;
using Hackathon.Abstraction.EventLog;
using Hackathon.Common.Models.EventLog;
using Microsoft.Extensions.Logging;

namespace Hackathon.BL.EventLog;

public class EventLogHandler: IEventLogHandler
{
    private readonly IEventLogService _service;
    private readonly ILogger<EventLogHandler> _logger;

    public EventLogHandler(
        IEventLogService service,
        ILogger<EventLogHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task Handle(EventLogModel logModel)
    {
        try
        {
            await _service.AddAsync(logModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while processing event log");
        }
    }
}
