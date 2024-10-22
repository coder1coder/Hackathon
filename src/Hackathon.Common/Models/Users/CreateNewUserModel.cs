namespace Hackathon.Common.Models.Users;

public class CreateNewUserModel
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }

    /// <summary>
    /// Данные учетной записи Google
    /// </summary>
    public GoogleAccountModel GoogleAccount { get; set; }
}
