using System.Threading.Tasks;
using Hackathon.Common.Models.Project;
using Hackathon.Contracts.Requests.Project;
using Hackathon.Contracts.Responses;
using Refit;

namespace Hackathon.API.Abstraction
{
    public interface IProjectApi
    {
        [Post("/api/Project")]
        Task<BaseCreateResponse> Create([Body] ProjectCreateRequest projectCreateRequest);

        [Put("/api/Project/git")]
        Task UpdateFromGit([Body] ProjectUpdateFromGitParameters parameters);

        [Get("/api/Project/{id}")]
        Task<ProjectModel> Get(long id);
    }
}
