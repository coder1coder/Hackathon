using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Project;
using Hackathon.DAL.Entities;
using MapsterMapper;

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

            _dbContext.Projects.Add(projectEntity);
            await _dbContext.SaveChangesAsync();
            return projectEntity.Id;
        }
    }
}