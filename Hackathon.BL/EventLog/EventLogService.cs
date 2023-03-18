using System.Threading.Tasks;
using Hackathon.Common.Abstraction.EventLog;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.EventLog;

namespace Hackathon.BL.EventLog;

public sealed class EventLogService: IEventLogService
{
    private readonly IEventLogRepository _repository;

    public EventLogService(IEventLogRepository repository)
    {
        _repository = repository;
    }

    public Task AddAsync(EventLogModel eventLogModel)
        => _repository.AddAsync(eventLogModel);

    public Task<BaseCollection<EventLogListItem>> GetListAsync(GetListParameters<EventLogModel> parameters)
        => _repository.GetListAsync(parameters);
}
