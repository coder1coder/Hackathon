using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hackathon.Abstraction.Project;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Project;
using Hackathon.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories
{
    public class ProjectRepository: IProjectRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public ProjectRepository(
            IMapper mapper,
            ApplicationDbContext dbContext
            )
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        /// <inheritdoc cref="IProjectRepository.CreateAsync(ProjectCreateModel)"/>
        public async Task<long> CreateAsync(ProjectCreateModel projectCreateModel)
        {
            var projectEntity = _mapper.Map<ProjectEntity>(projectCreateModel);
            await _dbContext.Projects.AddAsync(projectEntity);
            await _dbContext.SaveChangesAsync();
            return projectEntity.Id;
        }

        /// <inheritdoc cref="IProjectRepository.GetListAsync(GetListParameters{ProjectFilter})"/>
        public async Task<BaseCollection<ProjectListItem>> GetListAsync(GetListParameters<ProjectFilter> parameters)
        {
            var query = _dbContext.Projects
                .Include(x => x.Event)
                .Include(x => x.Team)
                .AsNoTracking()
                .AsQueryable();
            
            if (parameters.Filter?.Ids != null)
                query = query.Where(x=>
                    parameters.Filter.Ids.Contains(x.Id));
            
            if (parameters.Filter?.EventsIds != null)
                query = query.Where(x=>
                    parameters.Filter.EventsIds.Contains(x.EventId));
                
            if (parameters.Filter?.TeamsIds != null)
                query = query.Where(x=>
                        parameters.Filter.TeamsIds.Contains(x.TeamId));

            query = parameters.SortOrder == SortOrder.Asc
                ? query.OrderBy(ResolveOrderExpression(parameters))
                : query.OrderByDescending(ResolveOrderExpression(parameters));

            var totalCount = await query.LongCountAsync();

            return new BaseCollection<ProjectListItem>
            {
                Items = await query
                    .Skip(parameters.Offset)
                    .Take(parameters.Limit)
                    .Select(x=> new ProjectListItem
                    {
                        Id = x.Id,
                        Name = x.Name,
                        EventId = x.EventId,
                        TeamId = x.TeamId,
                        EventName = x.Event.Name,
                        TeamName = x.Team.Name
                    })
                    .ToArrayAsync(),
                TotalCount = totalCount
            };
        }

        private static Expression<Func<ProjectEntity, object>> ResolveOrderExpression(GetListParameters<ProjectFilter> parameters)
            => parameters.SortBy.ToLowerInvariant() switch
            {
                "eventId" => x => x.EventId,
                "teamId" => x =>x.TeamId,
                _ => x => x.Id
            };
    }
}