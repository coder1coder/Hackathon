using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Contracts.Requests.User;
using Refit;

namespace Hackathon.API.Abstraction
{
    public interface IAdminApi
    {
        [Post($"/api/Admin/{nameof(SignIn)}")]
        public Task<AuthTokenModel> SignIn([Body] SignInRequest request);
    }
}