using System.Net.Http;
using System.Threading.Tasks;
using Hackathon.API.Client.Base;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Contracts.Responses;

namespace Hackathon.API.Client.Team
{
    public class TeamApiClient: BaseApiClient, ITeamApiClient
    {
        private const string Endpoint = "api/Team";
        public TeamApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<BaseCreateResponse> Create(CreateTeamRequest createTeamRequest)
        {
            return await PostAsync<CreateTeamRequest, BaseCreateResponse>(Endpoint, createTeamRequest);
        }

        public async Task AddMember(TeamAddMemberRequest teamAddMemberRequest)
        {
            await PostAsync($"{Endpoint}/AddMember", teamAddMemberRequest);
        }
    }
}