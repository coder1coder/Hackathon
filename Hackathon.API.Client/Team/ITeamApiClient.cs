using System.Threading.Tasks;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Contracts.Responses;

namespace Hackathon.API.Client.Team
{
    public interface ITeamApiClient
    {
        Task<BaseCreateResponse> Create(CreateTeamRequest createTeamRequest);
        Task AddMember(TeamAddMemberRequest teamAddMemberRequest);
    }
}