using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using Hackathon.DAL.Entities;
using Hackathon.DAL.Mappings;
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
        
        /// <inheritdoc cref="ITeamRepository.CreateAsync(CreateTeamModel)"/>
        public async Task<long> CreateAsync(CreateTeamModel createTeamModel)
        {
            var createTeamEntity = _mapper.Map<TeamEntity>(createTeamModel);

            await _dbContext.AddAsync(createTeamEntity);
            await _dbContext.SaveChangesAsync();

            return createTeamEntity.Id;
        }

        /// <inheritdoc cref="ITeamRepository.GetAsync(long)"/>
        public async Task<TeamModel> GetAsync(long teamId)
        {
            var teamEntity = await _dbContext.Teams
                .AsNoTracking()
                .Include(x=>x.TeamEvents)
                .Include(x=>x.Users)
                .FirstOrDefaultAsync(x=>x.Id == teamId);

            if (teamEntity == null)
                throw new Exception("Команда с таким идентификатором не найдена");

            return _mapper.Map<TeamModel>(teamEntity);
        }

        /// <inheritdoc cref="ITeamRepository.GetAsync(GetFilterModel{TeamFilterModel})"/>
        public async Task<BaseCollectionModel<TeamModel>> GetAsync(GetFilterModel<TeamFilterModel> getFilterModel)
        {
            var query = _dbContext.Teams
                .AsNoTracking();

            if (getFilterModel.Filter != null)
            {
                if (getFilterModel.Filter.Ids != null)
                    query = query.Where(x => getFilterModel.Filter.Ids.Contains(x.Id));

                if (!string.IsNullOrWhiteSpace(getFilterModel.Filter.Name))
                    query = query.Where(x => x.Name == getFilterModel.Filter.Name);

                if (getFilterModel.Filter.EventId.HasValue)
                    query = query.Where(x => x.TeamEvents.Any(s => s.EventId == getFilterModel.Filter.EventId));

                if (getFilterModel.Filter.ProjectId.HasValue)
                    query = query.Where(x => x.TeamEvents.Any(s => s.ProjectId == getFilterModel.Filter.ProjectId));
            }

            var totalCount = await query.LongCountAsync();

            if (!string.IsNullOrWhiteSpace(getFilterModel.SortBy))
            {
                query = getFilterModel.SortBy switch
                {
                    nameof(TeamEntity.Name) => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Name)
                        : query.OrderByDescending(x => x.Name),

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
                .ProjectToType<TeamModel>(_mapper.Config)
                .ToListAsync();

            return new BaseCollectionModel<TeamModel>
            {
                Items = teamModels,
                TotalCount = totalCount
            };
        }

        /// <inheritdoc cref="ITeamRepository.ExistAsync(string)"/>
        public async Task<bool> ExistAsync(string teamName)
        {
            return await _dbContext.Teams
                .AsNoTracking()
                .AnyAsync(x => x.Name.ToLower() == teamName.ToLower());
        }

        /// <inheritdoc cref="ITeamRepository.ExistAsync(long)"/>
        public async Task<bool> ExistAsync(long teamId)
        {
            return await _dbContext.Teams
                .AsNoTracking()
                .AnyAsync(x => x.Id == teamId);
        }

        /// <inheritdoc cref="ITeamRepository.AddMemberAsync(TeamAddMemberModel)"/>
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