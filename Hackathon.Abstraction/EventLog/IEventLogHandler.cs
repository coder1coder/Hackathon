using Hackathon.Common.Models.EventLog;

namespace Hackathon.Abstraction.EventLog;

public interface IEventLogHandler
{
    Task Handle(EventLogModel logModel);
}