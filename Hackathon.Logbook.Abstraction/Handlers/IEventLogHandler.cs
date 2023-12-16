using System.Threading.Tasks;
using Hackathon.Logbook.Abstraction.Models;

namespace Hackathon.Logbook.Abstraction.Handlers;

public interface IEventLogHandler
{
    Task Handle(EventLogModel logModel);
}
