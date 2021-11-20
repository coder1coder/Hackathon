using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Project
{
    public class ProjectService: IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IValidator<ProjectCreateModel> _projectCreateModelValidator;

        public ProjectService(
            IProjectRepository projectRepository,
            IValidator<ProjectCreateModel> projectCreateModelValidator)
        {
            _projectRepository = projectRepository;
            _projectCreateModelValidator = projectCreateModelValidator;
        }

        public async Task<long> CreateAsync(ProjectCreateModel projectCreateModel)
        {
            await _projectCreateModelValidator.ValidateAndThrowAsync(projectCreateModel);
            return await _projectRepository.CreateAsync(projectCreateModel);
        }
    }
}