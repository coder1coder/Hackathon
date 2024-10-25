namespace Hackathon.Common.Models.Users;

/// <summary>
/// Сведения авторизованного пользователя
/// </summary>
public class UserSignInDetails
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long UserId { get; set; }
    
    /// <summary>
    /// Хеш пароля
    /// </summary>
    public string PasswordHash { get; set; }
    
    /// <summary>
    /// Роль пользователя
    /// </summary>
    public UserRole UserRole { get; set; }
    
    /// <summary>
    /// Идентификатор аккаунта Google
    /// </summary>
    public string GoogleAccountId { get; set; }
}
