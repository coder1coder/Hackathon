using System.Threading.Tasks;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.User;
using Hackathon.Contracts.Responses;
using Hackathon.Contracts.Responses.User;
using Refit;

namespace Hackathon.API.Abstraction
{
    public interface IUserApi
    {
        [Post("/api/User")]
        Task<BaseCreateResponse> SignUp([Body] SignUpRequest request);

        [Get("/api/User/{userId})")]
        public Task<UserResponse> Get(long userId);

        [Get("/api/User/{userId}/reactions")]
        public Task<UserProfileReaction[]> GetReactions(long userId);

        [Post("/api/User/{userId}/reactions/{reaction}")]
        public Task AddReaction(long userId, UserProfileReaction reaction);

        [Delete("/api/User/{userId}/reactions/{reaction}")]
        public Task RemoveReaction(long userId, UserProfileReaction reaction);
    }
}
