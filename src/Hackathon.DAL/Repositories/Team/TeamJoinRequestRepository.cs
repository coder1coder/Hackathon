using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.DAL.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hackathon.Common.Models.Teams;
using Hackathon.DAL.Entities.Teams;

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

    public async Task<long> CreateAsync(TeamJoinRequestCreateParameters createParameters)
    {
        var entity = _mapper.Map<TeamJoinRequestCreateParameters, TeamJoinRequestEntity>(createParameters);
        _dbContext.TeamJoinRequests.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity.Id;
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
            {
                query = query.Where(x => x.UserId == parameters.Filter.UserId.Value);
            }

            if (parameters.Filter.TeamId.HasValue)
            {
                query = query.Where(x => x.TeamId == parameters.Filter.TeamId.Value);
            }

            if (parameters.Filter.Status.HasValue)
            {
                query = query.Where(x => x.Status == parameters.Filter.Status.Value);
            }
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

    public async Task SetStatusWithCommentAsync(long joinRequestId, TeamJoinRequestStatus status, string comment = null)
    {
        var request = await _dbContext.TeamJoinRequests.FirstOrDefaultAsync(x => x.Id == joinRequestId);

        if (request is null)
        {
            return;
        }

        request.Status = status;
        request.Comment = comment;

        _dbContext.Update(request);
        await _dbContext.SaveChangesAsync();
    }

    public Task<TeamJoinRequestModel> GetAsync(long requestId)
        => _dbContext.TeamJoinRequests.AsNoTracking()
            .Include(x => x.Team)
            .Include(x => x.User)
            .ProjectToType<TeamJoinRequestModel>()
            .FirstOrDefaultAsync(x => x.Id == requestId);

    private static Expression<Func<TeamJoinRequestEntity, object>> ResolveOrderFieldExpression(PaginationSort parameters)
        => parameters.SortBy?.ToLowerInvariant() switch
        {
            "teamId" => x => x.TeamId,
            "status" => x =>x.Status,
            "createdAt" => x =>x.CreatedAt,
            _ => x => x.Id
        };
}
