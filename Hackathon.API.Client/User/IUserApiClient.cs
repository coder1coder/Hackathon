using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Contracts.Requests.User;
using Hackathon.Contracts.Responses;

namespace Hackathon.API.Client.User
{
    public interface IUserApiClient
    {
        Task<BaseCreateResponse> SignUpAsync(SignUpRequest request);
        Task<AuthTokenModel> SignInAsync(SignInRequest request);
    }
}