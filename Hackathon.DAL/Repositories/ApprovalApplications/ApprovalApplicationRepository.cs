using System.Linq;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Common.Abstraction.ApprovalApplications;
using Hackathon.Common.Models.ApprovalApplications;
using Hackathon.Common.Models.Event;
using Hackathon.DAL.Entities.ApprovalApplications;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories.ApprovalApplications;

public class ApprovalApplicationRepository: IApprovalApplicationRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ApprovalApplicationRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
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

    public async Task<Page<ApprovalApplicationModel>> GetListAsync(GetListParameters<ApprovalApplicationFilter> parameters)
    {
        var query = _dbContext.ApprovalApplications.AsQueryable();

        if (parameters.Filter is not null)
        {
            if (parameters.Filter.EventId.HasValue)
                query = query.Where(x =>
                    x.Event.Id == parameters.Filter.EventId.Value);

            if (parameters.Filter.Status.HasValue)
                query = query.Where(x =>
                    x.ApplicationStatus == parameters.Filter.Status);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .Include(x=>x.Event)
            .Include(x=>x.Author)
            .Include(x=>x.Signer)
            .AsNoTracking()
            .Take(parameters.Limit)
            .Skip(parameters.Offset)
            .OrderByDescending(x => x.RequestedAt)
            .ToArrayAsync();

        return new Page<ApprovalApplicationModel>{
            Items = _mapper.Map<ApprovalApplicationEntity[], ApprovalApplicationModel[]>(items),
            Total = totalCount
            };
    }
}
