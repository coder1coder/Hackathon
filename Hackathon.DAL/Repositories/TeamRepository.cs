using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hackathon.Abstraction.Team;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using Hackathon.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories
{
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

            if (teamEntity == null)
                throw new Exception("Команда с таким идентификатором не найдена");

            return _mapper.Map<TeamModel>(teamEntity);
        }

        public async Task<BaseCollection<TeamModel>> GetAsync(GetListParameters<TeamFilter> parameters)
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
            }

            var totalCount = await query.LongCountAsync();

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                query = parameters.SortBy switch
                {
                    nameof(TeamEntity.Name) => parameters.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Name)
                        : query.OrderByDescending(x => x.Name),

                    _ => parameters.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Id)
                        : query.OrderByDescending(x => x.Id)
                };
            }

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

        public async Task<bool> ExistAsync(string teamName)
            => await _dbContext.Teams
                .AsNoTracking()
                .AnyAsync(x => x.Name.ToLower() == teamName.ToLower());

        public async Task<bool> ExistAsync(long teamId)
            => await _dbContext.Teams
                .AsNoTracking()
                .AnyAsync(x => x.Id == teamId);

        public async Task AddMemberAsync(TeamMemberModel teamMemberModel)
        {
            var team = await _dbContext.Teams
                .SingleOrDefaultAsync(x=>x.Id == teamMemberModel.TeamId);

            if (team == null)
                throw new EntityNotFoundException("Команда с указаным индентификатором не найдена");

            var user = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Id == teamMemberModel.MemberId);

            if (user == null)
                throw new EntityNotFoundException("Пользователь с указаным индентификатором не найден");

            team.Members.Add(new MemberTeamEntity
            {
                TeamId = teamMemberModel.TeamId,
                Team = team,
                MemberId = teamMemberModel.MemberId,
                Member = user,
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(TeamMemberModel teamMemberModel)
        {
            var team = await _dbContext.Teams
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x =>
                    x.Id == teamMemberModel.TeamId);

            if (team == null)
                throw new EntityNotFoundException("Команда с указаным индентификатором не найдена");

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == teamMemberModel.MemberId);

            if (user == null)
                throw new EntityNotFoundException("Пользователь с указаным индентификатором не найден");

            var teamMember = team.Members.FirstOrDefault(x => x.MemberId == user.Id);

            if (teamMember == null)
                throw new EntityNotFoundException("Пользователь не состоит в команде");

            team.Members.Remove(teamMember);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TeamModel[]> GetByExpressionAsync(
            Expression<Func<TeamEntity, bool>> expression,
            string[] includes = null)
        {
            var query = _dbContext.Teams
                .Where(expression)
                .IgnoreAutoIncludes();

            if (includes is {Length: > 0})
                foreach (var include in includes)
                    query.Include(include);

            return await query
                .AsNoTracking()
                .ProjectToType<TeamModel>(_mapper.Config)
                .ToArrayAsync();
        }

        public async Task ChangeTeamOwnerAsync(ChangeOwnerModel changeOwnerModel)
        {
            var teamEntity = await _dbContext.Teams
                .SingleOrDefaultAsync(
                    x => (x.Id == changeOwnerModel.TeamId)
                    && (x.OwnerId == changeOwnerModel.OwnerId));

            if (teamEntity == null)
                throw new EntityNotFoundException("Команда с указаным индентификатором и владельцем не найдена");

            teamEntity.OwnerId = changeOwnerModel.NewOwnerId;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTeamAsync(DeleteTeamModel deleteTeamModel)
        {
            var teamEntity = await _dbContext.Teams
                .SingleOrDefaultAsync(x => (x.Id == deleteTeamModel.TeamId));

            if (teamEntity == null)
                throw new EntityNotFoundException("Команда с указаным индентификатором не найдена");

            teamEntity.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetMembersCountAsync(long teamId)
        {
            var teamEntity = await _dbContext.Teams
                .Include(team => team.Members)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == teamId);

            if (teamEntity == null)
                throw new EntityNotFoundException("Команда с указаным индентификатором не найдена");

            var teamMembers = teamEntity.Members.Count();

            if (teamEntity.Owner != null)
                teamMembers += 1;

            return teamMembers;
        }
    }
}
