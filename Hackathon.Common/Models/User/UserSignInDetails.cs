namespace Hackathon.Common.Models.User;

public class UserSignInDetails
{
    public long UserId { get; set; }
    public string PasswordHash { get; set; }
    public UserRole UserRole { get; set; }
    public string GoogleAccountId { get; set; }
}
