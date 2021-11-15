using System.Threading.Tasks;
using Hackathon.Contracts.Requests.Project;
using Hackathon.Contracts.Responses;

namespace Hackathon.API.Client.Project
{
    public interface IProjectApiClient
    {
        Task<BaseCreateResponse> CreateAsync(ProjectCreateRequest projectCreateRequest);
    }
}