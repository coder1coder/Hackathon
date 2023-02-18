using System.Threading.Tasks;
using Hackathon.Common.Models.Project;
using Hackathon.Contracts.Requests.Project;
using Hackathon.Contracts.Responses;
using Refit;

namespace Hackathon.API.Abstraction;

public interface IProjectApi
{
    private const string BaseRoute = "/api/Project";

    [Post(BaseRoute)]
    Task<BaseCreateResponse> Create([Body] ProjectCreateRequest projectCreateRequest);

    [Put(BaseRoute + "/git")]
    Task UpdateFromGit([Body] ProjectUpdateFromGitParameters parameters);

    [Get(BaseRoute + "/{id}")]
    Task<ProjectModel> Get(long id);
}