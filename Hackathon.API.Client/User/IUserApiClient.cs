using System.Threading.Tasks;
using Hackathon.Contracts.Requests.User;
using Hackathon.Contracts.Responses;

namespace Hackathon.API.Client.User
{
    public interface IUserApiClient
    {
        Task<BaseCreateResponse> CreateAsync(SignUpRequest request);
    }
}