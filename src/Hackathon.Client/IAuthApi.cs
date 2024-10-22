using System.Threading.Tasks;
using Hackathon.API.Contracts.Users;
using Hackathon.Common.Models;
using Refit;

namespace Hackathon.Client;

public interface IAuthApi
{
    private const string BaseRoute = "/api/Auth";

    [Post(BaseRoute + "/SignIn")]
    Task<AuthTokenModel> SignIn([Body] SignInRequest request);
}
