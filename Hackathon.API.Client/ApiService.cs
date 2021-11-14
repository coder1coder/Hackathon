using System.Net.Http;
using Hackathon.API.Client.User;

namespace Hackathon.API.Client
{
    public class ApiService: IApiService
    {
        public IUserApiClient Users { get; }

        public ApiService(HttpClient httpClient)
        {
            Users = new UserApiClient(httpClient);
        }
    }
}