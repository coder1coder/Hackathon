using System.Linq;
using System.Threading.Tasks;
using Hackathon.Abstraction;
using Hackathon.Common.Models.Project;
using Hackathon.DAL.Entities;
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

            var team = await _dbContext.Teams
                .Include(x=>x.TeamEvents)
                .SingleAsync(x => x.Id == projectCreateModel.TeamId
                                  && x.TeamEvents.Any(s=>s.EventId == projectCreateModel.EventId));

            var teamEntity = team.TeamEvents.Single(x => x.EventId == projectCreateModel.EventId);

            teamEntity.Project = projectEntity;
            await _dbContext.SaveChangesAsync();
            return projectEntity.Id;
        }
    }
}