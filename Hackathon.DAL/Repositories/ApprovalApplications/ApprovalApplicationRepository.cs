using System.Threading.Tasks;
using Hackathon.Common.Abstraction.ApprovalApplications;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories.ApprovalApplications;

public class ApprovalApplicationRepository: IApprovalApplicationRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ApprovalApplicationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task RemoveAsync(long approvalApplicationId)
    {
        var entity = await _dbContext.ApprovalApplications
            .FirstOrDefaultAsync(x =>
                x.Id == approvalApplicationId);

        if (entity is not null)
        {
            _dbContext.ApprovalApplications.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
