using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Logbook.Abstraction.Models;
using Refit;

namespace Hackathon.Client;

public interface IEventLogApi
{
    private const string BaseRoute = "/api/eventLog";

    [Post(BaseRoute + "/list")]
    Task<BaseCollection<EventLogModel>> GetListAsync([Body] GetListParameters<EventLogModel> parameters);
}
