using System.Net.Http;
using System.Threading.Tasks;
using Hackathon.API.Client.Base;
using Hackathon.Contracts.Requests.User;
using Hackathon.Contracts.Responses;

namespace Hackathon.API.Client.User
{
    public class UserApiClient: BaseApiClient, IUserApiClient
    {
        private const string Endpoint = "api/User";
        public UserApiClient(HttpClient httpClient) : base(Endpoint, httpClient)
        {
        }

        public Task<BaseCreateResponse> CreateAsync(SignUpRequest request)
        {
            return base.CreateAsync<SignUpRequest, BaseCreateResponse>(request);
        }
    }
}