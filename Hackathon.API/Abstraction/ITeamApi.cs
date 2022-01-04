using System.Threading.Tasks;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Contracts.Responses;
using Refit;

namespace Hackathon.API.Abstraction
{
    public interface ITeamApi
    {
        [Post("/api/Team")]
        Task<BaseCreateResponse> Create([Body] CreateTeamRequest createTeamRequest);
        [Post("/api/Team/AddMember")]
        Task AddMember([Body] TeamAddMemberRequest teamAddMemberRequest);
    }
}