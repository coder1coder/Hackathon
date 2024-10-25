namespace Hackathon.Common.Models.Users;

/// <summary>
/// Параметры создания нового пользователя
/// </summary>
public class CreateNewUserModel
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Полное имя пользователя
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Данные учетной записи Google
    /// </summary>
    public GoogleAccountModel GoogleAccount { get; set; }
}
