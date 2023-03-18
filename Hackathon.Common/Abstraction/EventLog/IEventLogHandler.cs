using Hackathon.Common.Models.EventLog;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.EventLog;

public interface IEventLogHandler
{
    Task Handle(EventLogModel logModel);
}