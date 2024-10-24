﻿using BackendTools.Common.Models;
using System.Threading.Tasks;
using Hackathon.Common.Models.Projects;

namespace Hackathon.Common.Abstraction.Project;

public interface IProjectService
{
    /// <summary>
    /// Создание проекта
    /// </summary>
    /// <param name="authorizedUserId"></param>
    /// <param name="projectCreationParameters"></param>
    Task<Result> CreateAsync(long authorizedUserId, ProjectCreationParameters projectCreationParameters);

    /// <summary>
    /// Обновить проект
    /// </summary>
    Task<Result> UpdateAsync(long authorizedUserId, ProjectUpdateParameters parameters);

    /// <summary>
    /// Обновить файлы проекта из Git-репозитория
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="branchParameters"></param>
    Task<Result> UpdateProjectFromGitBranchAsync(long userId, UpdateProjectFromGitBranchParameters branchParameters);

    /// <summary>
    /// Получить проект по идентификатору
    /// </summary>
    /// <param name="authorizedUserId">Идентификатор авторизованного пользователя</param>
    /// <param name="eventId">Идентификатор проекта</param>
    /// /// <param name="teamId">Идентификатор команды</param>
    Task<Result<ProjectModel>> GetAsync(long authorizedUserId, long eventId, long teamId);

    /// <summary>
    /// Удалить проект
    /// </summary>
    /// <param name="authorizedUserId"></param>
    /// <param name="eventId"></param>
    /// <param name="teamId"></param>
    Task<Result> DeleteAsync(long authorizedUserId, long eventId, long teamId);
}
