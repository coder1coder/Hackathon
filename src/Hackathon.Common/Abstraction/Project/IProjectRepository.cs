using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using System.Threading.Tasks;
using Hackathon.Common.Models.Projects;

namespace Hackathon.Common.Abstraction.Project;

/// <summary>
/// Проекты
/// </summary>
public interface IProjectRepository
{
    /// <summary>
    /// Создать проект
    /// </summary>
    /// <param name="projectCreationParameters">Параметры</param>
    Task CreateAsync(ProjectCreationParameters projectCreationParameters);

    /// <summary>
    /// Обновить проект
    /// </summary>
    /// <param name="parameters"></param>
    Task UpdateAsync(ProjectUpdateParameters parameters);

    /// <summary>
    /// Обновить информацию обновления проекта из Git-репозитория
    /// </summary>
    /// <param name="uploadingFromGitInfo"></param>
    Task UpdateUploadingFromGitInfo(ProjectUploadingFromGitInfoDto uploadingFromGitInfo);

    /// <summary>
    /// Получить список проектов
    /// </summary>
    /// <param name="parameters">Параметры</param>
    Task<BaseCollection<ProjectListItem>> GetListAsync(GetListParameters<ProjectFilter> parameters);

    /// <summary>
    /// Получить проект
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="includeTeamMembers">Включить в результат участников команды</param>
    Task<ProjectModel> GetAsync(long eventId, long teamId, bool includeTeamMembers = false);

    /// <summary>
    /// Удалить проект
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <param name="teamId">Идентификатор команды</param>
    Task DeleteAsync(long eventId, long teamId);
}
