using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Abstraction.EventLog;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.EventLog;

namespace Hackathon.BL.EventLog;

public sealed class EventLogService: IEventLogService
{
    private readonly IEventLogRepository _repository;
    private readonly IMessageBusService _messageBusService;

    public EventLogService(
        IEventLogRepository repository, 
        IMessageBusService messageBusService)
    {
        _repository = repository;
        _messageBusService = messageBusService;
    }

    public Task AddAsync(EventLogModel eventLogModel)
        => _messageBusService.TryPublish(eventLogModel);

    public Task<BaseCollection<EventLogListItem>> GetListAsync(GetListParameters<EventLogModel> parameters)
        => _repository.GetListAsync(parameters);
}
