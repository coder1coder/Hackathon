using System.Threading.Tasks;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Contracts.Responses;
using Refit;

namespace Hackathon.API.Abstraction
{
    public interface ITeamApi
    {
        [Post("/v1/Team")]
        Task<BaseCreateResponse> Create([Body] CreateTeamRequest createTeamRequest);
    }
}