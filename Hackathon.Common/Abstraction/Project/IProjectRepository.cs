using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Project;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Project;

/// <summary>
/// Проекты
/// </summary>
public interface IProjectRepository
{
    /// <summary>
    /// Создать проект
    /// </summary>
    /// <param name="projectCreateParameters">Параметры</param>
    /// <returns></returns>
    Task CreateAsync(ProjectCreateParameters projectCreateParameters);

    /// <summary>
    /// Обновить проект
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task UpdateAsync(ProjectUpdateParameters parameters);

    /// <summary>
    /// Обновить информацию обновления проекта из Git-репозитория
    /// </summary>
    /// <param name="uploadingFromGitInfo"></param>
    /// <returns></returns>
    Task UpdateUploadingFromGitInfo(ProjectUploadingFromGitInfoDto uploadingFromGitInfo);

    /// <summary>
    /// Получить список проектов
    /// </summary>
    /// <param name="parameters">Параметры</param>
    /// <returns></returns>
    Task<BaseCollection<ProjectListItem>> GetListAsync(GetListParameters<ProjectFilter> parameters);

    /// <summary>
    /// Получить проект
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <param name="teamId">Идентификатор команды</param>
    /// <returns></returns>
    Task<ProjectModel> GetAsync(long eventId, long teamId);

    /// <summary>
    /// Удалить проект
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <param name="teamId">Идентификатор команды</param>
    /// <returns></returns>
    Task DeleteAsync(long eventId, long teamId);
}
