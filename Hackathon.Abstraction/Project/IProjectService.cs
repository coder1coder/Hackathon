using Hackathon.Common.Models.Project;

namespace Hackathon.Abstraction.Project
{
    public interface IProjectService
    {
        /// <summary>
        /// Создание проекта
        /// </summary>
        /// <param name="projectCreateParameters"></param>
        /// <returns></returns>
        Task<long> CreateAsync(ProjectCreateParameters projectCreateParameters);

        /// <summary>
        /// Обновить проект из гит репозитория
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="parameters">Параметры</param>
        /// <returns></returns>
        Task UpdateFromGitAsync(long userId, ProjectUpdateFromGitParameters parameters);

        /// <summary>
        /// Получить модель по идентификатору
        /// </summary>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <returns></returns>
        Task<ProjectModel> GetAsync(long projectId);
    }
}
