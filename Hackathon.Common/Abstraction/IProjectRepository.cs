using System.Threading.Tasks;
using Hackathon.Common.Models.Project;

namespace Hackathon.Common.Abstraction
{
    public interface IProjectRepository
    {
        Task<long> CreateAsync(ProjectCreateModel projectCreateModel);
    }
}