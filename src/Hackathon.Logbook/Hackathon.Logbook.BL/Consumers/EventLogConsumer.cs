using System.Threading.Tasks;
using Hackathon.Logbook.Abstraction.Handlers;
using Hackathon.Logbook.Abstraction.Models;
using MassTransit;

namespace Hackathon.Logbook.BL.Consumers;

public class EventLogConsumer: IConsumer<EventLogModel>
{
    private readonly IEventLogHandler _logHandler;

    public EventLogConsumer(IEventLogHandler logHandler)
    {
        _logHandler = logHandler;
    }

    public Task Consume(ConsumeContext<EventLogModel> context)
        => _logHandler.Handle(context.Message);
}
