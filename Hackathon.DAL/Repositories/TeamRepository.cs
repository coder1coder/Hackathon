using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using Hackathon.DAL.Entities;
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

            await _dbContext.AddAsync(createTeamEntity);
            await _dbContext.SaveChangesAsync();

            return createTeamEntity.Id;
        }

        public async Task<TeamModel> GetAsync(long teamId)
        {
            var teamEntity = await _dbContext.Teams
                .AsNoTracking()
                .Include(x=>x.Event)
                .Include(x=>x.Users)
                .Include(x=> x.Project)
                .FirstOrDefaultAsync(x=>x.Id == teamId);

            if (teamEntity == null)
                throw new Exception("Команда с таким идентификатором не найдена");

            return _mapper.Map<TeamModel>(teamEntity);
        }

        public async Task<BaseCollectionModel<TeamModel>> GetAsync(GetFilterModel<TeamFilterModel> getFilterModel)
        {
            var query = _dbContext.Teams.AsNoTracking();

            if (getFilterModel.Filter != null)
            {
                if (getFilterModel.Filter.Ids != null)
                    query = query.Where(x => getFilterModel.Filter.Ids.Contains(x.Id));

                if (!string.IsNullOrWhiteSpace(getFilterModel.Filter.Name))
                    query = query.Where(x => x.Name == getFilterModel.Filter.Name);

                if (getFilterModel.Filter.EventId.HasValue)
                    query = query.Where(x => x.EventId == getFilterModel.Filter.EventId);

                if (getFilterModel.Filter.ProjectId.HasValue)
                    query = query.Where(x => x.ProjectId == getFilterModel.Filter.ProjectId);
            }

            var totalCount = await query.LongCountAsync();

            if (!string.IsNullOrWhiteSpace(getFilterModel.SortBy))
            {
                query = getFilterModel.SortBy switch
                {
                    nameof(TeamEntity.Name) => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Name)
                        : query.OrderByDescending(x => x.Name),

                    nameof(TeamEntity.EventId) => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.EventId)
                        : query.OrderByDescending(x => x.EventId),

                    nameof(TeamEntity.ProjectId) => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.ProjectId)
                        : query.OrderByDescending(x => x.ProjectId),

                    _ => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Id)
                        : query.OrderByDescending(x => x.Id)
                };
            }

            var page = getFilterModel.Page ?? 1;
            var pageSize = getFilterModel.PageSize ?? 1000;

            var teamModels = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectToType<TeamModel>()
                .ToListAsync();

            return new BaseCollectionModel<TeamModel>
            {
                Items = teamModels,
                TotalCount = totalCount
            };
        }

        public async Task<bool> ExistAsync(string teamName)
        {
            return await _dbContext.Teams
                .AsNoTracking()
                .AnyAsync(x => x.Name.ToLower() == teamName.ToLower());
        }

        public async Task<bool> ExistAsync(long teamId)
        {
            return await _dbContext.Teams
                .AsNoTracking()
                .AnyAsync(x => x.Id == teamId);
        }

        public async Task AddMemberAsync(TeamAddMemberModel teamAddMemberModel)
        {
            //clear changeTracker because it contains team
            _dbContext.ChangeTracker.Clear();

            var teamEntity = await _dbContext.Teams
                .SingleOrDefaultAsync(x=>x.Id == teamAddMemberModel.TeamId);

            if (teamEntity == null)
                throw new EntityNotFoundException("Команда с указаным индентификатором не найдена");

            var userEntity = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Id == teamAddMemberModel.UserId);

            if (userEntity == null)
                throw new EntityNotFoundException("Пользователь с указаным индентификатором не найден");

            teamEntity.Users.Add(userEntity);

            await _dbContext.SaveChangesAsync();
        }
    }
}