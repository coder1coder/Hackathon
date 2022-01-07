using Hackathon.Common.Configuration;
using Microsoft.Extensions.Options;

namespace Hackathon.BL.Auth;

public class AuthService: IAuthService
{
    private readonly AuthOptions _authConfig;

    public long? UserId { get; set; }

    public AuthService(IOptions<AuthOptions> authOptions)
    {
        _authConfig = authOptions.Value;
    }
}