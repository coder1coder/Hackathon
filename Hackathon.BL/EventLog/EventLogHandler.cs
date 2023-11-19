using System;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.EventLog;
using Hackathon.Common.Models.EventLog;
using Microsoft.Extensions.Logging;

namespace Hackathon.BL.EventLog;

public class EventLogHandler: IEventLogHandler
{
    private readonly IEventLogRepository _repository;
    private readonly ILogger<EventLogHandler> _logger;

    public EventLogHandler(
        ILogger<EventLogHandler> logger, 
        IEventLogRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task Handle(EventLogModel eventLogModel)
    {
        try
        {
            await _repository.AddAsync(eventLogModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Initiator}. Ошибка во время обработки сообщения журнала событий",
                nameof(EventLogHandler));
            throw;
        }
    }
}
