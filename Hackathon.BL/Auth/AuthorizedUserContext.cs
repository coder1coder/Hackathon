using System.Security.Claims;
using Hackathon.Common.Abstraction.Auth;
using Hackathon.Common.Models.Auth;
using Microsoft.AspNetCore.Http;

namespace Hackathon.BL.Auth;

public class AuthorizedUserContext: IAuthorizedUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizedUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public AuthorizedUser GetAuthorizedUser()
    {
        var nameIdentifier = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(nameIdentifier, out var userId)
            ? new AuthorizedUser
            {
                Id = userId
            }
            : null;
    }
}
