using System.Security.Claims;

namespace Hackathon.Common.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static long? GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        return long.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId)
            ? userId
            : null;
    }
}
