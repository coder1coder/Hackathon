namespace Hackathon.Configuration;

/// <summary>
/// Настройки аутентификации и авторизации
/// </summary>
public class AuthOptions
{
    /// <summary>
    /// Издатель
    /// </summary>
    public string Issuer { get; set; }
    
    /// <summary>
    /// Потребитель
    /// </summary>
    public string Audience { get; set; }
    
    /// <summary>
    /// Время жизни токена
    /// </summary>
    public int LifeTime { get; set; }
    
    /// <summary>
    /// Секрет
    /// </summary>
    public string Secret { get; set; }
}
