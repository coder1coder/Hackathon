using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models;

public class AuthTokenModel
{
    public long UserId { get; set; }
    public string Token { get; set; }
    public long Expires { get; set; }

    public string GoogleId { get; set; }

    public UserRole Role { get; set; }
}
