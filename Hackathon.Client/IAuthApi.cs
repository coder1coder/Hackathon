using Hackathon.Common.Models;
using Hackathon.Contracts.Requests.User;
using Refit;

namespace Hackathon.Client;

public interface IAuthApi
{
    private const string BaseRoute = "/api/Auth";

    [Post(BaseRoute + "/SignIn")]
    public Task<AuthTokenModel> SignIn([Body] SignInRequest request);
}
