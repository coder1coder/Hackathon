using System.Threading.Tasks;
using Hackathon.Contracts.Requests.ProjectMember;
using Hackathon.Contracts.Responses;
using Refit;

namespace Hackathon.API.Abstraction;

public interface IProjectMemberApi
{
    [Post("/api/ProjectMember")]
    Task<BaseCreateResponse> Create([Body] CreateProjectMemberRequest createProjectMemberRequest);

}