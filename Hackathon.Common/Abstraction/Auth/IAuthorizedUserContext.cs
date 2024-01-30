using Hackathon.Common.Models.Auth;

namespace Hackathon.Common.Abstraction.Auth;

public interface IAuthorizedUserContext
{
    AuthorizedUser GetAuthorizedUser();
}
