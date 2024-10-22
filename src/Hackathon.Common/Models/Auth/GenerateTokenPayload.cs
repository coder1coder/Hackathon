using Hackathon.Common.Models.Users;

namespace Hackathon.Common.Models.Auth;

public class GenerateTokenPayload
{
    public long UserId { get; set; }

    public UserRole UserRole { get; set; }
    
    public string GoogleAccountId { get; set; }
}
