namespace Hackathon.Configuration.Auth;

/// <summary>
/// Настройки аутентификации
/// </summary>
public class AuthenticateSettings
{
    /// <summary>
    /// Настройки встренной аутентификации
    /// </summary>
    public InternalAuthenticateSettings Internal { get; set; }
    
    /// <summary>
    /// Настройки внешней аутентификации
    /// </summary>
    public ExternalAuthenticateSettings External { get; set; }
}
