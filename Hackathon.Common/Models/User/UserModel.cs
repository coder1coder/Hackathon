namespace Hackathon.Common.Models.User;

public class UserModel: UserShortModel
{
    /// <summary>
    /// Email пользователя
    /// </summary>
    public UserEmailModel Email { get; set; } = new();

    public GoogleAccountModel GoogleAccount { get; set; }
}
