using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using Hackathon.DAL.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hackathon.DAL.Repositories.Team;

public class TeamJoinRequestRepository: ITeamJoinRequestsRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public TeamJoinRequestRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task CreateAsync(TeamJoinRequestCreateParameters createParameters)
    {
        var entity = _mapper.Map<TeamJoinRequestCreateParameters, TeamJoinRequestEntity>(createParameters);
        _dbContext.TeamJoinRequests.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<BaseCollection<TeamJoinRequestModel>> GetListAsync(GetListParameters<TeamJoinRequestExtendedFilter> parameters)
    {
        var query = _dbContext.TeamJoinRequests
            .Include(x => x.Team)
            .Include(x=>x.User)
            .AsNoTracking();

        if (parameters.Filter is not null)
        {
            if (parameters.Filter.UserId.HasValue)
                query = query.Where(x => x.UserId == parameters.Filter.UserId.Value);

            if (parameters.Filter.TeamId.HasValue)
                query = query.Where(x => x.TeamId == parameters.Filter.TeamId.Value);

            if (parameters.Filter.Status.HasValue)
                query = query.Where(x => x.Status == parameters.Filter.Status.Value);
        }

        var totalCount = await query.CountAsync();

        query = parameters.SortOrder == SortOrder.Asc
            ? query.OrderBy(ResolveOrderFieldExpression(parameters))
            : query.OrderByDescending(ResolveOrderFieldExpression(parameters));

        var entities = await query
            .Skip(parameters.Offset)
            .Take(parameters.Limit)
            .ToArrayAsync();

        return new BaseCollection<TeamJoinRequestModel>
        {
            Items = _mapper.Map<TeamJoinRequestEntity[], TeamJoinRequestModel[]>(entities),
            TotalCount = totalCount
        };
    }

    public async Task SetStatusAsync(long joinRequestId, TeamJoinRequestStatus status)
    {
        var request = await _dbContext.TeamJoinRequests.FirstOrDefaultAsync(x => x.Id == joinRequestId);

        if (request is null)
            return;

        request.Status = status;

        _dbContext.Update(request);
        await _dbContext.SaveChangesAsync();
    }

    private static Expression<Func<TeamJoinRequestEntity, object>> ResolveOrderFieldExpression(PaginationSort parameters)
        => parameters.SortBy?.ToLowerInvariant() switch
        {
            "teamId" => x => x.TeamId,
            "status" => x =>x.Status,
            "createdAt" => x =>x.CreatedAt,
            _ => x => x.Id
        };
}
