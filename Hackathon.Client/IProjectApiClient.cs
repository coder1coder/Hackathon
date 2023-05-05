using Hackathon.Common.Models.Project;
using Hackathon.Contracts.Requests.Project;
using Refit;

namespace Hackathon.Client;

public interface IProjectApiClient
{
    private const string BaseRoute = "/api/Project";

    [Post(BaseRoute)]
    Task CreateAsync([Body] ProjectCreateRequest projectCreateRequest);

    [Put(BaseRoute)]
    Task UpdateAsync([Body] ProjectUpdateParameters parameters);

    [Get(BaseRoute + "/{eventId}/{teamId}")]
    Task<ProjectModel> GetAsync(long eventId, long teamId);

    /// <summary>
    /// Обновить проект из ветки Git-репозитория
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Put(BaseRoute + "/branch")]
    Task UpdateProjectFromGitBranch([Body] UpdateProjectFromGitBranchRequest request);
}
