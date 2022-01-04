using System.Threading.Tasks;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.User;
using Hackathon.Contracts.Responses;
using Refit;

namespace Hackathon.API.Abstraction
{
    public interface IUserApi
    {
        [Post("/api/User")]
        Task<BaseCreateResponse> SignUp([Body] SignUpRequest request);

        [Get("/api/User/{userId})")]
        public Task<UserModel> Get(long userId);
    }
}