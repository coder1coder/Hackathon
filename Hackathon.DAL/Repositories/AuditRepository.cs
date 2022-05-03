using System.Threading.Tasks;
using Hackathon.Abstraction.Audit;
using Hackathon.Abstraction.Entities;

namespace Hackathon.DAL.Repositories;

public class AuditRepository: IAuditRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AuditRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="IAuditRepository.AddAsync"/>
    public async Task AddAsync(AuditEventEntity auditEventEntity)
    {
        _dbContext.Audit.Add(auditEventEntity);
        await _dbContext.SaveChangesAsync();
    }
}