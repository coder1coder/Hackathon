using System.Net.Http;
using System.Threading.Tasks;
using Hackathon.API.Client.Base;
using Hackathon.Common.Models;
using Hackathon.Contracts.Requests.User;
using Hackathon.Contracts.Responses;

namespace Hackathon.API.Client.User
{
    public class UserApiClient: BaseApiClient, IUserApiClient
    {
        private const string Endpoint = "api/User";
        public UserApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public Task<BaseCreateResponse> SignUpAsync(SignUpRequest request)
        {
            return PostAsync<SignUpRequest, BaseCreateResponse>(Endpoint, request, true);
        }

        public Task<AuthTokenModel> SignInAsync(SignInRequest request)
        {
            return PostAsync<SignInRequest, AuthTokenModel>($"{Endpoint}/SignIn", request, true);
        }
    }
}