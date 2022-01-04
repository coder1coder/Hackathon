using System.Threading.Tasks;
using Hackathon.Contracts.Requests.Project;
using Hackathon.Contracts.Responses;
using Refit;

namespace Hackathon.API.Abstraction
{
    public interface IProjectApi
    {
        [Post("/api/Project")]
        Task<BaseCreateResponse> Create([Body] ProjectCreateRequest projectCreateRequest);
    }
}