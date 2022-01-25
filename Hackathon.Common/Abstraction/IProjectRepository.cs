using System.Threading.Tasks;
using Hackathon.Common.Models.Project;

namespace Hackathon.Common.Abstraction
{
    public interface IProjectRepository
    {
        /// <summary>
        /// Создание проекта
        /// </summary>
        /// <param name="projectCreateModel"></param>
        /// <returns></returns>
        Task<long> CreateAsync(ProjectCreateModel projectCreateModel);
    }
}