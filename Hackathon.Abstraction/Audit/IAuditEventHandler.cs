using Hackathon.Common.Models.Audit;

namespace Hackathon.Abstraction.Audit;

public interface IAuditEventHandler
{
    Task Handle(AuditEventModel model);
}