using System.IO;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Abstraction.FileStorage;
using Hackathon.Abstraction.IntegrationServices;
using Hackathon.Abstraction.Project;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Project
{
    public class ProjectService: IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IValidator<ProjectCreateParameters> _projectCreateModelValidator;
        private readonly IValidator<ProjectUpdateFromGitParameters> _projectUpdateFromGitValidator;
        private readonly IGitHubIntegrationService _gitHubIntegrationService;
        private readonly IFileStorageService _fileStorageService;

        public ProjectService(
            IProjectRepository projectRepository,
            IValidator<ProjectCreateParameters> projectCreateModelValidator,
            IValidator<ProjectUpdateFromGitParameters> projectUpdateFromGitValidator,
            IGitHubIntegrationService gitHubIntegrationService,
            IFileStorageService fileStorageService)
        {
            _projectRepository = projectRepository;
            _projectCreateModelValidator = projectCreateModelValidator;
            _projectUpdateFromGitValidator = projectUpdateFromGitValidator;
            _gitHubIntegrationService = gitHubIntegrationService;
            _fileStorageService = fileStorageService;
        }

        public async Task<long> CreateAsync(ProjectCreateParameters projectCreateParameters)
        {
            await _projectCreateModelValidator.ValidateAndThrowAsync(projectCreateParameters);
            return await _projectRepository.CreateAsync(projectCreateParameters);
        }

        public async Task UpdateFromGitAsync(long userId, ProjectUpdateFromGitParameters parameters)
        {
            await _projectUpdateFromGitValidator.ValidateAndThrowAsync(parameters);

            var project = await _projectRepository.GetAsync(parameters.ProjectId);

            if (project is null)
                throw new ValidationException("Проект не существует");

            var gitParameters = _gitHubIntegrationService.ParseFromLink(parameters.LinkToGitBranch);

            var stream = await _gitHubIntegrationService.ReceiveFromRepository(gitParameters);
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);

            if (memoryStream is null || !memoryStream.CanRead)
                throw new ValidationException("Ошибка чтения из репозитория");

            if (memoryStream.CanSeek) memoryStream.Seek(0, SeekOrigin.Begin);

            var storageFile = await _fileStorageService.UploadAsync(memoryStream, Bucket.Projects,
                $"{gitParameters.UserName}_{gitParameters.Repository}_{gitParameters.Branch}.zip", userId);

            project.FileIds.Add(storageFile.Id);

            await _projectRepository.UpdateAsync(project);
        }

        public Task<ProjectModel> GetAsync(long projectId)
        {
            return _projectRepository.GetAsync(projectId);
        }
    }
}
