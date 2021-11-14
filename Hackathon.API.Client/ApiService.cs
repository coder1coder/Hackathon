using System.Net.Http;
using Hackathon.API.Client.Event;
using Hackathon.API.Client.User;

namespace Hackathon.API.Client
{
    public class ApiService: IApiService
    {
        public IUserApiClient Users { get; }
        public IEventClient Events { get; }

        public ApiService(HttpClient httpClient)
        {
            Users = new UserApiClient(httpClient);
            Events = new EventApiClient(httpClient);
        }
    }
}