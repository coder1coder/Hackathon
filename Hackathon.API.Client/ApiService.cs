using System.Net.Http;
using Hackathon.API.Client.Event;
using Hackathon.API.Client.Team;
using Hackathon.API.Client.User;

namespace Hackathon.API.Client
{
    public class ApiService: IApiService
    {
        public IUserApiClient Users { get; }
        public IEventApiClient Events { get; }
        public ITeamApiClient Teams { get; }

        public ApiService(HttpClient httpClient)
        {
            Users = new UserApiClient(httpClient);
            Events = new EventApiClient(httpClient);
            Teams = new TeamApiClient(httpClient);
        }
    }
}