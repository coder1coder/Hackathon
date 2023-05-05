using BackendTools.Common.Models;
using Hackathon.Common.Models.Project;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Project;

public interface IProjectService
{
    /// <summary>
    /// Создание проекта
    /// </summary>
    /// <param name="projectCreateParameters"></param>
    /// <returns></returns>
    Task<Result> CreateAsync(ProjectCreateParameters projectCreateParameters);

    /// <summary>
    /// Обновить проект
    /// </summary>
    /// <returns></returns>
    Task<Result> UpdateAsync(ProjectUpdateParameters parameters, long userId);

    /// <summary>
    /// Обновить файлы проекта из Git-репозитория
    /// </summary>
    /// <param name="branchParameters"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Result> UpdateProjectFromGitBranchAsync(UpdateProjectFromGitBranchParameters branchParameters, long userId);

    /// <summary>
    /// Получить проект по идентификатору
    /// </summary>
    /// <param name="eventId">Идентификатор проекта</param>
    /// /// <param name="teamId">Идентификатор команды</param>
    /// <returns></returns>
    Task<ProjectModel> GetAsync(long eventId, long teamId);

    /// <summary>
    /// Удалить проект
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="teamId"></param>
    /// <param name="authorizedUserId"></param>
    /// <returns></returns>
    Task<Result> DeleteAsync(long eventId, long teamId, long authorizedUserId);
}
