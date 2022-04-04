using System.Threading.Tasks;
using Hackathon.Contracts.Requests.ProjectMemberRole;
using Hackathon.Contracts.Responses;
using Refit;

namespace Hackathon.API.Abstraction;

public interface IProjectMemberRoleApi
{
    [Post("/api/ProjectMemberRole")]
        Task<BaseCreateResponse> Create([Body] CreateProjectMemberRoleRequest createProjectMemberRoleRequest);
}