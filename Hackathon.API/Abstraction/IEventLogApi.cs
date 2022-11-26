using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.EventLog;
using Refit;

namespace Hackathon.API.Abstraction;

public interface IEventLogApi
{
    [Post("/api/EventLog/list")]
    Task<BaseCollection<EventLogModel>> GetListAsync([Body] GetListParameters<EventLogModel> parameters);
}
