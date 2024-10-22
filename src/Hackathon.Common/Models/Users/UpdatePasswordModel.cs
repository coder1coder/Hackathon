namespace Hackathon.Common.Models.Users;

/// <summary>
/// Модель обновления пароля пользователя
/// </summary>
public class UpdatePasswordModel
{
    /// <summary>
    /// Текущий пароль
    /// </summary>
    public string CurrentPassword { get; set; }
    
    /// <summary>
    /// Новый пароль
    /// </summary>
    public string NewPassword { get; set; }
}
