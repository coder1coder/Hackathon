namespace Hackathon.Configuration;

/// <summary>
/// Настройки аутентификации и авторизации
/// </summary>
public class AuthOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int LifeTime { get; set; }
    public string Secret { get; set; }
}