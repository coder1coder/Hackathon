using System.Threading.Tasks;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Contracts.Responses;
using Refit;

namespace Hackathon.API.Abstraction
{
    public interface ITeamApi
    {
        private const string BaseRoute = "/api/Team";

        [Post(BaseRoute)]
        Task<IApiResponse<BaseCreateResponse>> Create([Body] CreateTeamRequest createTeamRequest);

        [Get(BaseRoute + "/{id}")]
        Task<TeamModel> Get(long id);
    }
}
