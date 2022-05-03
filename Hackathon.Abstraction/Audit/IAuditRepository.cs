using Hackathon.Entities;

namespace Hackathon.Abstraction.Audit;

public interface IAuditRepository
{
    /// <summary>
    /// Добавить событие аудита
    /// </summary>
    /// <param name="auditEventEntity"></param>
    /// <returns></returns>
    Task AddAsync(AuditEventEntity auditEventEntity);
}