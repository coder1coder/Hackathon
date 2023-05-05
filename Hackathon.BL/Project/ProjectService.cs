using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.Common.Abstraction.FileStorage;
using Hackathon.Common.Abstraction.Project;
using Hackathon.Common.ErrorMessages;
using Hackathon.Common.Models.Project;
using Hackathon.IntegrationServices.Github.Abstraction;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Hackathon.BL.Project;

public class ProjectService: IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IValidator<ProjectCreateParameters> _projectCreateModelValidator;
    private readonly IValidator<ProjectUpdateParameters> _projectUpdateValidator;
    private readonly IValidator<UpdateProjectFromGitBranchParameters> _updateProjectFromGitValidator;

    private readonly IGitHubIntegrationService _gitHubIntegrationService;
    private readonly IFileStorageService _fileStorageService;

    public ProjectService(
        IProjectRepository projectRepository,
        IValidator<ProjectCreateParameters> projectCreateModelValidator,
        IValidator<ProjectUpdateParameters> projectUpdateValidator,
        IValidator<UpdateProjectFromGitBranchParameters> updateProjectFromGitValidator,
        IGitHubIntegrationService gitHubIntegrationService,
        IFileStorageService fileStorageService)
    {
        _projectRepository = projectRepository;
        _projectCreateModelValidator = projectCreateModelValidator;
        _gitHubIntegrationService = gitHubIntegrationService;
        _fileStorageService = fileStorageService;
        _updateProjectFromGitValidator = updateProjectFromGitValidator;
        _projectUpdateValidator = projectUpdateValidator;
    }

    public async Task<Result> CreateAsync(ProjectCreateParameters projectCreateParameters)
    {
        await _projectCreateModelValidator.ValidateAndThrowAsync(projectCreateParameters);

        var project = await _projectRepository.GetAsync(projectCreateParameters.EventId, projectCreateParameters.TeamId);

        if (project is not null)
            return Result.NotValid(ProjectMessages.ProjectAlreadyExists);

        await _projectRepository.CreateAsync(projectCreateParameters);
        return Result.Success;
    }

    public async Task<Result> UpdateAsync(ProjectUpdateParameters parameters, long userId)
    {
        await _projectUpdateValidator.ValidateAndThrowAsync(parameters);

        var project = await _projectRepository.GetAsync(parameters.EventId, parameters.TeamId);

        if (project is null)
            return Result.NotFound(ProjectMessages.ProjectDoesNotExist);

        await _projectRepository.UpdateAsync(parameters);
        return Result.Success;
    }

    public async Task<Result> UpdateProjectFromGitBranchAsync(UpdateProjectFromGitBranchParameters branchParameters, long userId)
    {
        await _updateProjectFromGitValidator.ValidateAndThrowAsync(branchParameters);

        var project = await _projectRepository.GetAsync(branchParameters.EventId, branchParameters.TeamId);

        if (project is null)
            return Result.NotFound(ProjectMessages.ProjectDoesNotExist);

        var projectUploadingFromGitInfoDto = new ProjectUploadingFromGitInfoDto
        {
            EventId = branchParameters.EventId,
            TeamId = branchParameters.TeamId,
            LinkToGitBranch = branchParameters.LinkToGitBranch
        };

        //TODO: добавить ограничение на обновление не чаще раз в 5-10 минут
        if (!string.IsNullOrWhiteSpace(branchParameters.LinkToGitBranch))
        {
            var uploadResult = await UploadGitRepositoryAsArchiveAsync(branchParameters.LinkToGitBranch, userId);

            if (!uploadResult.IsSuccess)
                return Result.FromErrors(uploadResult.Errors);

            projectUploadingFromGitInfoDto.FileIds = new[] { uploadResult.Data };
        }

        await _projectRepository.UpdateUploadingFromGitInfo(projectUploadingFromGitInfoDto);

        if (string.IsNullOrWhiteSpace(project.LinkToGitBranch) && project.FileIds is { Length: > 0})
        {
            foreach (var oldFileId in project.FileIds)
            {
                await _fileStorageService.DeleteAsync(oldFileId);
            }
        }

        return Result.Success;
    }

    public Task<ProjectModel> GetAsync(long eventId, long teamId)
        => _projectRepository.GetAsync(eventId, teamId);

    public async Task<Result> DeleteAsync(long eventId, long teamId, long authorizedUserId)
    {
        var project = await _projectRepository.GetAsync(eventId, teamId);

        if (project is null)
            return Result.NotFound(ProjectMessages.ProjectDoesNotExist);

        //TODO: Ограничить права на удаление, когда будет капитан команды
        //return Result.Forbidden("Нет прав на удаление проекта");

        await _projectRepository.DeleteAsync(eventId, teamId);
        return Result.Success;
    }

    /// <summary>
    /// Получить наименование файла проекта на основе параметров
    /// </summary>
    /// <param name="gitParameters">Параметры</param>
    /// <returns></returns>
    private static string GetProjectFileName(GitParameters gitParameters)
        => $"{gitParameters.UserName}_{gitParameters.Repository}_{gitParameters.Branch}.zip";

    /// <summary>
    /// Получить файлы из ветки Git-репозитория и загрузить их архивом в файловое хранилище
    /// </summary>
    /// <param name="linkToGitBranch"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    private async Task<Result<Guid>> UploadGitRepositoryAsArchiveAsync(string linkToGitBranch, long userId)
    {
        var gitParameters = _gitHubIntegrationService.ParseFromLink(linkToGitBranch);

        var stream = await _gitHubIntegrationService.ReceiveFromRepository(gitParameters);
        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);

        if (!memoryStream.CanRead)
            return Result<Guid>.Internal(ProjectMessages.ReadFromRepositoryError);

        if (memoryStream.CanSeek) memoryStream.Seek(0, SeekOrigin.Begin);

        var storageFile = await _fileStorageService
            .UploadAsync(memoryStream, Bucket.Projects, GetProjectFileName(gitParameters), userId);

        return Result<Guid>.FromValue(storageFile.Id);
    }
}
