using Hackathon.Common.Models.User;

namespace Hackathon.Contracts.Responses.User;

public class UserResponse
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }

    public GoogleAccountModel GoogleAccount { get; set; }
    public UserRole Role { get; set; } = UserRole.Default;
}