using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.Auth;

public class GenerateTokenPayload
{
    public long UserId { get; set; }

    public UserRole UserRole { get; set; }
    
    public string GoogleAccountId { get; set; }
}
