using System.Threading.Tasks;
using Hackathon.Common.Messages;
using Hackathon.Logbook.Abstraction.Models;
using Hackathon.Logbook.Abstraction.Services;
using MassTransit;

namespace Hackathon.Logbook.BL.Consumers;

public class EventCreatedMessageConsumer: IConsumer<EventCreatedMessage>
{
    private readonly IEventLogService _eventLogService;

    public EventCreatedMessageConsumer(IEventLogService eventLogService)
    {
        _eventLogService = eventLogService;
    }

    public async Task Consume(ConsumeContext<EventCreatedMessage> context)
    {
        await _eventLogService.AddAsync(new EventLogModel(
            EventLogType.Created,
            $"Добавлено новое событие с идентификатором '{context.Message.EventId}'",
            context.Message.UserId
        ));
    }
}
