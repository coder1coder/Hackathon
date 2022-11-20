using System.Threading.Tasks;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Contracts.Responses;
using Refit;

namespace Hackathon.API.Abstraction
{
    public interface ITeamApi
    {
        [Post("/api/Team")]
        Task<BaseCreateResponse> Create([Body] CreateTeamRequest createTeamRequest);

        [Get("/api/Team/{id}")]
        Task<TeamModel> Get(long id);
    }
}
