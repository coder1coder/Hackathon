namespace Hackathon.Common.Models.Users;

public class UserModel: UserShortModel
{
    /// <summary>
    /// Email пользователя
    /// </summary>
    public UserEmailModel Email { get; set; } = new();

    public GoogleAccountModel GoogleAccount { get; set; }
}
