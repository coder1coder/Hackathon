using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Logbook.Abstraction.Models;

namespace Hackathon.Logbook.Abstraction.Services;

public interface IEventLogService
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
