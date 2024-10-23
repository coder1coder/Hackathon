using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Logbook.Abstraction.Models;

namespace Hackathon.Logbook.Abstraction.Repositories;

public interface IEventLogRepository
{
    /// <summary>
    /// Добавить запись в журнал событий
    /// </summary>
    /// <param name="eventLogModel"></param>
    Task AddAsync(EventLogModel eventLogModel);

    /// <summary>
    /// Получить список записей журнала событий
    /// </summary>
    /// <param name="parameters"></param>
    Task<BaseCollection<EventLogListItem>> GetListAsync(GetListParameters<EventLogModel> parameters);
}
