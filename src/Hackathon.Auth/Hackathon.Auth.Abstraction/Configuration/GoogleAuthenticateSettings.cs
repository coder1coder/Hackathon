namespace Hackathon.Auth.Abstraction.Configuration;

/// <summary>
/// Аутентификация с помощью Google
/// </summary>
public class GoogleAuthenticateSettings
{
    /// <summary>
    /// Потребитель токена
    /// </summary>
    public string Audience { get; set; }
}
