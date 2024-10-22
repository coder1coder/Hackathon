namespace Hackathon.Common.Models.Users;

public class UserSignInDetails
{
    public long UserId { get; set; }
    public string PasswordHash { get; set; }
    public UserRole UserRole { get; set; }
    public string GoogleAccountId { get; set; }
}
