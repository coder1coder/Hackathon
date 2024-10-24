﻿using System;
using System.IO;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.Common.Abstraction.Project;
using Hackathon.Common.Models.Projects;
using Hackathon.FileStorage.Abstraction.Models;
using Hackathon.FileStorage.Abstraction.Services;
using Hackathon.IntegrationServices.Github.Abstraction;

namespace Hackathon.BL.Projects;

public class ProjectService: IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly Hackathon.Common.Abstraction.IValidator<ProjectCreationParameters> _createValidator;
    private readonly Hackathon.Common.Abstraction.IValidator<ProjectUpdateParameters> _projectUpdateValidator;
    private readonly IValidator<UpdateProjectFromGitBranchParameters> _updateProjectFromGitValidator;

    private readonly IGitHubIntegrationService _gitHubIntegrationService;
    private readonly IFileStorageService _fileStorageService;

    public ProjectService(
        IProjectRepository projectRepository,
        Hackathon.Common.Abstraction.IValidator<ProjectCreationParameters> createValidator,
        Hackathon.Common.Abstraction.IValidator<ProjectUpdateParameters> projectUpdateValidator,
        IValidator<UpdateProjectFromGitBranchParameters> updateProjectFromGitValidator,
        IGitHubIntegrationService gitHubIntegrationService,
        IFileStorageService fileStorageService)
    {
        _projectRepository = projectRepository;
        _createValidator = createValidator;
        _gitHubIntegrationService = gitHubIntegrationService;
        _fileStorageService = fileStorageService;
        _updateProjectFromGitValidator = updateProjectFromGitValidator;
        _projectUpdateValidator = projectUpdateValidator;
    }

    public async Task<Result> CreateAsync(long authorizedUserId, ProjectCreationParameters projectCreationParameters)
    {
        var validationResult = await _createValidator.ValidateAsync(projectCreationParameters, authorizedUserId);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        await _projectRepository.CreateAsync(projectCreationParameters);
        return Result.Success;
    }

    public async Task<Result> UpdateAsync(long authorizedUserId, ProjectUpdateParameters parameters)
    {
        var validationResult = await _projectUpdateValidator.ValidateAsync(parameters, authorizedUserId);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        await _projectRepository.UpdateAsync(parameters);
        return Result.Success;
    }

    public async Task<Result> UpdateProjectFromGitBranchAsync(long userId,
        UpdateProjectFromGitBranchParameters branchParameters)
    {
        await _updateProjectFromGitValidator.ValidateAndThrowAsync(branchParameters);

        var project = await _projectRepository.GetAsync(branchParameters.EventId, branchParameters.TeamId);

        if (project is null)
        {
            return Result.NotFound(ProjectErrorMessages.ProjectDoesNotExist);
        }

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
            {
                return Result.FromErrors(uploadResult.Errors);
            }

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

    public async Task<Result<ProjectModel>> GetAsync(long authorizedUserId, long eventId, long teamId)
    {
        var projectModel = await _projectRepository.GetAsync(eventId, teamId, includeTeamMembers: true);

        if (projectModel is null)
        {
            return Result<ProjectModel>.NotFound(ProjectErrorMessages.ProjectDoesNotExist);
        }

        return !projectModel.Team.HasMemberWithId(authorizedUserId)
            ? Result<ProjectModel>.Forbidden("Нет прав на выполнение операции")
            : Result<ProjectModel>.FromValue(projectModel);
    }

    public async Task<Result> DeleteAsync(long authorizedUserId, long eventId, long teamId)
    {
        var project = await _projectRepository.GetAsync(eventId, teamId);

        if (project is null)
        {
            return Result.NotFound(ProjectErrorMessages.ProjectDoesNotExist);
        }

        //TODO: Ограничить права на удаление, когда будет капитан команды
        if (authorizedUserId == default)
        {
            return Result.Forbidden("Нет прав на удаление проекта");
        }

        await _projectRepository.DeleteAsync(eventId, teamId);
        return Result.Success;
    }

    /// <summary>
    /// Получить наименование файла проекта на основе параметров
    /// </summary>
    /// <param name="gitParameters">Параметры</param>
    private static string GetProjectFileName(GitParameters gitParameters)
        => $"{gitParameters.UserName}_{gitParameters.Repository}_{gitParameters.Branch}.zip";

    /// <summary>
    /// Получить файлы из ветки Git-репозитория и загрузить их архивом в файловое хранилище
    /// </summary>
    /// <param name="linkToGitBranch"></param>
    /// <param name="userId"></param>
    private async Task<Result<Guid>> UploadGitRepositoryAsArchiveAsync(string linkToGitBranch, long userId)
    {
        var gitParameters = _gitHubIntegrationService.ParseFromLink(linkToGitBranch);

        var stream = await _gitHubIntegrationService.ReceiveFromRepository(gitParameters);
        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);

        if (!memoryStream.CanRead)
        {
            return Result<Guid>.Internal(ProjectErrorMessages.ReadFromRepositoryError);
        }

        if (memoryStream.CanSeek)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
        }

        var storageFile = await _fileStorageService
            .UploadAsync(memoryStream, Bucket.Projects, GetProjectFileName(gitParameters), userId);

        return Result<Guid>.FromValue(storageFile.Id);
    }
}
