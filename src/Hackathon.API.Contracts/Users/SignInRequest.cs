namespace Hackathon.API.Contracts.Users;

public class SignInRequest
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
}
