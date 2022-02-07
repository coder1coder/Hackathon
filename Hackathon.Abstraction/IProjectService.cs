using Hackathon.Common.Models.Project;

namespace Hackathon.Abstraction
{
    public interface IProjectService
    {
        /// <summary>
        /// Создание проекта
        /// </summary>
        /// <param name="projectCreateModel"></param>
        /// <returns></returns>
        Task<long> CreateAsync(ProjectCreateModel projectCreateModel);
    }
}