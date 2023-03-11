using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.EventLog;
using Refit;

namespace Hackathon.Client;

public interface IEventLogApi
{
    private const string BaseRoute = "/api/EventLog";

    [Post(BaseRoute+"/list")]
    Task<BaseCollection<EventLogModel>> GetListAsync([Body] GetListParameters<EventLogModel> parameters);
}
