using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using Hackathon.DAL.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hackathon.DAL.Repositories.Team;

public class TeamRepository : ITeamRepository
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _dbContext;
    public TeamRepository(IMapper mapper, ApplicationDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<long> CreateAsync(CreateTeamModel createTeamModel)
    {
        var createTeamEntity = _mapper.Map<TeamEntity>(createTeamModel);

        if (createTeamModel.EventId.HasValue)
        {
            var eventEntity = await _dbContext.Events
                .FirstOrDefaultAsync(x =>
                    x.Id == createTeamModel.EventId.Value);

            eventEntity?.Teams.Add(createTeamEntity);
        }
        else
        {
            await _dbContext.AddAsync(createTeamEntity);
        }

        await _dbContext.SaveChangesAsync();

        return createTeamEntity.Id;
    }

    public async Task<TeamModel> GetAsync(long teamId)
    {
        var teamEntity = await _dbContext.Teams
            .AsNoTracking()
            .Include(x=>x.Owner)
            .Include(x=>x.Members)
                .ThenInclude(x=>x.Member)
            .SingleOrDefaultAsync(x=>x.Id == teamId);

        return teamEntity is null
            ? null
            : _mapper.Map<TeamEntity, TeamModel>(teamEntity);
    }

    public async Task<BaseCollection<TeamModel>> GetListAsync(GetListParameters<TeamFilter> parameters)
    {
        var query = _dbContext.Teams
            .Include(x => x.Members)
            .Include(x => x.Events)
            .AsNoTracking();

        if (parameters.Filter != null)
        {
            if (parameters.Filter.Ids != null)
                query = query.Where(x => parameters.Filter.Ids.Contains(x.Id));

            if (!string.IsNullOrWhiteSpace(parameters.Filter.Name))
                query = query.Where(x => x.Name.ToLower().Contains(parameters.Filter.Name.ToLower().Trim()));

            if (!string.IsNullOrWhiteSpace(parameters.Filter.Owner))
                query = query.Where(x => x.Owner.FullName.ToLower().Contains(parameters.Filter.Owner.ToLower().Trim()));

            if (parameters.Filter.QuantityUsersFrom.HasValue)
                query = query.Where(x =>
                    x.Members.Count >= parameters.Filter.QuantityUsersFrom);

            if (parameters.Filter.QuantityUsersTo.HasValue)
                query = query.Where(x =>
                    x.Members.Count <= parameters.Filter.QuantityUsersTo);

            if (parameters.Filter.EventId.HasValue)
                query = query.Where(x => x.Events.Any(s => s.Id == parameters.Filter.EventId));

            if (parameters.Filter.OwnerId.HasValue)
                query = query.Where(x => x.OwnerId == parameters.Filter.OwnerId);

            if (parameters.Filter.HasOwner.HasValue)
                query = query.Where(x => x.OwnerId != null);

            if (parameters.Filter.MemberId.HasValue)
                query = query.Where(x=>
                    x.OwnerId != null && (x.OwnerId == parameters.Filter.MemberId || x.Members.Any(s => s.MemberId == parameters.Filter.MemberId)));

            if (parameters.Filter.TeamType.HasValue)
                query = query.Where(x => x.Type == parameters.Filter.TeamType);
        }

        var totalCount = await query.LongCountAsync();

        query = parameters.SortOrder == SortOrder.Asc
            ? query.OrderBy(ResolveOrderFieldExpression(parameters))
            : query.OrderByDescending(ResolveOrderFieldExpression(parameters));

        var teamModels = await query
            .Skip(parameters.Offset)
            .Take(parameters.Limit)
            .ProjectToType<TeamModel>(_mapper.Config)
            .ToListAsync();

        return new BaseCollection<TeamModel>
        {
            Items = teamModels,
            TotalCount = totalCount
        };
    }

    public Task<bool> ExistAsync(string teamName)
        => _dbContext.Teams
            .AsNoTracking()
            .AnyAsync(x => x.Name.ToLower() == teamName.ToLower());

    public Task<bool> ExistAsync(long teamId)
        => _dbContext.Teams
            .AsNoTracking()
            .AnyAsync(x => x.Id == teamId);

    public async Task AddMemberAsync(TeamMemberModel teamMemberModel)
    {
        var team = await _dbContext.Teams
            .FirstOrDefaultAsync(x=>x.Id == teamMemberModel.TeamId);

        if (team is not null)
        {
            var memberTeamEntity = new MemberTeamEntity
            {
                TeamId = teamMemberModel.TeamId,
                MemberId = teamMemberModel.MemberId
            };

            _dbContext.Entry(memberTeamEntity).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveMemberAsync(TeamMemberModel teamMemberModel)
    {
        var team = await _dbContext.Teams
            .Include(x => x.Members)
            .FirstOrDefaultAsync(x =>
                x.Id == teamMemberModel.TeamId);

        var teamMember = team?.Members?.FirstOrDefault(x => x.MemberId == teamMemberModel.MemberId);

        if (teamMember is not null)
        {
            team.Members.Remove(teamMember);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task SetOwnerAsync(long teamId, long ownerId)
    {
        var teamEntity = await _dbContext.Teams.FirstOrDefaultAsync(x => x.Id == teamId);

        if (teamEntity is not null)
        {
            teamEntity.OwnerId = ownerId;

            _dbContext.Teams.Update(teamEntity);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteTeamAsync(DeleteTeamModel deleteTeamModel)
    {
        var teamEntity = await _dbContext.Teams
            .SingleOrDefaultAsync(x => x.Id == deleteTeamModel.TeamId);

        if (teamEntity is not null)
        {
            teamEntity.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<int> GetMembersCountAsync(long teamId)
    {
        var teamEntity = await _dbContext.Teams
            .Include(team => team.Members)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == teamId);

        if (teamEntity is null)
            return default;

        var teamMembers = teamEntity.Members.Count;

        if (teamEntity.Owner != null)
            teamMembers += 1;

        return teamMembers;
    }

    private static Expression<Func<TeamEntity, object>> ResolveOrderFieldExpression(PaginationSort parameters)
        => parameters.SortBy switch
        {
            nameof(TeamEntity.Name) => x => x.Name,
            _ => x => x.Id
        };
}
