using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Contracts.Requests.User;
using Refit;

namespace Hackathon.API.Abstraction
{
    public interface IAuthApi
    {
        [Post($"/v1/Auth/{nameof(SignIn)}")]
        public Task<AuthTokenModel> SignIn([Body] SignInRequest request);
    }
}