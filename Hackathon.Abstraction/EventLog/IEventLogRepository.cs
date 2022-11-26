using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.EventLog;

namespace Hackathon.Abstraction.EventLog;

public interface IEventLogRepository
{
    /// <summary>
    /// Добавить запись в журнал событий
    /// </summary>
    /// <param name="eventLogModel"></param>
    /// <returns></returns>
    Task AddAsync(EventLogModel eventLogModel);

    /// <summary>
    /// Получить список записей журнала событий
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<BaseCollection<EventLogListItem>> GetListAsync(GetListParameters<EventLogModel> parameters);
}
