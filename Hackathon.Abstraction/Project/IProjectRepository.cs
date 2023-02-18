using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Project;

namespace Hackathon.Abstraction.Project;

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
    Task<long> CreateAsync(ProjectCreateParameters projectCreateParameters);

    /// <summary>
    /// Обновить проект
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task UpdateAsync(ProjectUpdateParameters parameters);

    /// <summary>
    /// Получить список проектов
    /// </summary>
    /// <param name="parameters">Параметры</param>
    /// <returns></returns>
    Task<BaseCollection<ProjectListItem>> GetListAsync(GetListParameters<ProjectFilter> parameters);

    /// <summary>
    /// Получить проект по идентификатору
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <returns></returns>
    Task<ProjectModel> GetAsync(long projectId);

    /// <summary>
    /// Проверяет существование проекта по идентификатору
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <returns></returns>
    Task<bool> Exists(long projectId);
}