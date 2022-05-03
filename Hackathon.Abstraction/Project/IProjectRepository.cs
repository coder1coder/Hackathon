using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Project;

namespace Hackathon.Abstraction.Project
{
    /// <summary>
    /// Проекты
    /// </summary>
    public interface IProjectRepository
    {
        /// <summary>
        /// Создание проекта
        /// </summary>
        /// <param name="projectCreateModel"></param>
        /// <returns></returns>
        Task<long> CreateAsync(ProjectCreateModel projectCreateModel);

        /// <summary>
        /// Получить список проектов
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<BaseCollection<ProjectListItem>> GetListAsync(GetListParameters<ProjectFilter> parameters);
    }
}