using System;
using System.Threading.Tasks;
using Hackathon.Abstraction.Audit;
using Hackathon.Common.Models.Audit;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Hackathon.API;

public class AuditEventConsumer: IConsumer<AuditEventModel>
{
    private readonly IAuditEventHandler _handler;
    private readonly ILogger<AuditEventConsumer> _logger;

    public AuditEventConsumer(IAuditEventHandler handler, ILogger<AuditEventConsumer> logger)
    {
        _handler = handler;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AuditEventModel> context)
    {
        try
        {
            await _handler.Handle(context.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Произошла ошибка при обработке события аудита");
        }
    }
}

