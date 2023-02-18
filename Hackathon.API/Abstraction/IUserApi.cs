using System.Threading.Tasks;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.User;
using Hackathon.Contracts.Responses;
using Hackathon.Contracts.Responses.User;
using Refit;

namespace Hackathon.API.Abstraction;

public interface IUserApi
{
    private const string BaseRoute = "/api/User";

    [Post(BaseRoute)]
    Task<BaseCreateResponse> SignUp([Body] SignUpRequest request);

    [Get(BaseRoute + "/{userId})")]
    public Task<UserResponse> Get(long userId);

    [Get(BaseRoute + "/{userId}/reactions")]
    public Task<UserProfileReaction> GetReactions(long userId);

    [Post(BaseRoute + "/{userId}/reactions/{reaction}")]
    public Task AddReaction(long userId, UserProfileReaction reaction);

    [Delete(BaseRoute + "/{userId}/reactions/{reaction}")]
    public Task RemoveReaction(long userId, UserProfileReaction reaction);
}