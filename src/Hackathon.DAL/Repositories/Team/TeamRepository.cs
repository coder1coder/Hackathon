using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.DAL.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hackathon.Common.Models.Teams;
using Hackathon.DAL.Entities.Teams;

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

        if (createTeamModel.EventId.HasValue) //Случай, когда команда создается автоматически
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
                .ThenInclude(x=>x.Member)
            .Include(x=>x.Owner)
            .Include(x => x.Events)
            .AsNoTracking();

        if (parameters.Filter != null)
        {
            if (parameters.Filter.Ids != null)
            {
                query = query.Where(x => parameters.Filter.Ids.Contains(x.Id));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Filter.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(parameters.Filter.Name.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Filter.Owner))
            {
                query = query.Where(x => x.Owner.FullName.ToLower().Contains(parameters.Filter.Owner.ToLower().Trim()));
            }

            if (parameters.Filter.QuantityUsersFrom.HasValue)
            {
                query = query.Where(x =>
                    x.Members.Count >= parameters.Filter.QuantityUsersFrom);
            }

            if (parameters.Filter.QuantityUsersTo.HasValue)
            {
                query = query.Where(x =>
                    x.Members.Count <= parameters.Filter.QuantityUsersTo);
            }

            if (parameters.Filter.EventId.HasValue)
            {
                query = query.Where(x => x.Events.Any(s => s.Id == parameters.Filter.EventId));
            }

            if (parameters.Filter.OwnerId.HasValue)
            {
                query = query.Where(x => x.OwnerId == parameters.Filter.OwnerId);
            }

            if (parameters.Filter.HasOwner.HasValue)
            {
                query = query.Where(x => x.OwnerId != null);
            }

            if (parameters.Filter.MemberId.HasValue)
            {
                query = query.Where(x=>
                    x.OwnerId != null && (x.OwnerId == parameters.Filter.MemberId || x.Members.Any(s => s.MemberId == parameters.Filter.MemberId)));
            }

            if (parameters.Filter.TeamType.HasValue)
            {
                query = query.Where(x => x.Type == parameters.Filter.TeamType);
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

        return new BaseCollection<TeamModel>
        {
            Items = _mapper.Map<TeamEntity[], TeamModel[]>(entities),
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
                MemberId = teamMemberModel.MemberId,
                Role = teamMemberModel.Role
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
        var teamEntity = await _dbContext.Teams
            .Include(x => x.Members)
            .FirstOrDefaultAsync(x => x.Id == teamId);

        if (teamEntity is not null)
        {
            MemberTeamEntity teamMemberNewOwner = null, teamMemberOldOwner = null;

            foreach (var member in teamEntity.Members)
            {
                if (member.MemberId == ownerId)
                {
                    teamMemberNewOwner = member; //Поиск нового владельца
                }

                if (member.Role == TeamRole.Owner)
                {
                    teamMemberOldOwner = member; //Поиск старого владельца
                }
            }

            if (teamMemberNewOwner is not null && teamMemberOldOwner is not null
                && teamMemberNewOwner != teamMemberOldOwner)
            {
                _dbContext.TeamMembers.Remove(teamMemberOldOwner);

                teamMemberNewOwner.Role = TeamRole.Owner;
                teamEntity.OwnerId = ownerId;

                _dbContext.Teams.Update(teamEntity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

    public async Task DeleteTeamAsync(DeleteTeamModel deleteTeamModel)
    {
        var teamEntity = await _dbContext.Teams
            .Include(x => x.Members)
            .SingleOrDefaultAsync(x => x.Id == deleteTeamModel.TeamId);

        if (teamEntity is not null)
        {
            teamEntity.IsDeleted = true;
            teamEntity.Members.Clear();
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<int> GetMembersCountAsync(long teamId)
    {
        //TODO: изменить на код ниже когда будет выполнен тикет #320 
        // _dbContext.TeamMembers.CountAsync(x=>
        //     x.TeamId == teamId)
        
        var teamEntity = await _dbContext.Teams
            .Include(team => team.Members)
            .Include(teamEntity => teamEntity.Owner)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == teamId);

        if (teamEntity is null)
        {
            return default;
        }

        var teamMembers = teamEntity.Members.Count;

        if (teamEntity.Owner != null)
        {
            teamMembers += 1;
        }

        return teamMembers;
    }

    public Task<long[]> GetTeamMemberIdsAsync(long teamId, long? excludeMemberId = null)
        => _dbContext.TeamMembers
            .Where(x=>
                x.TeamId == teamId
                && (!excludeMemberId.HasValue || x.MemberId == excludeMemberId))
            .Select(x=>x.MemberId)
            .ToArrayAsync();

    private static Expression<Func<TeamEntity, object>> ResolveOrderFieldExpression(PaginationSort parameters)
        => parameters.SortBy switch
        {
            nameof(TeamEntity.Name) => x => x.Name,
            _ => x => x.Id
        };
}
