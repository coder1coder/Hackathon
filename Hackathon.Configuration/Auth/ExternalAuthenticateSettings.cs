namespace Hackathon.Configuration.Auth;

/// <summary>
/// Настройки внешней аутентификации
/// </summary>
public class ExternalAuthenticateSettings
{
    /// <summary>
    /// Аутентификация с помощью Google
    /// </summary>
    public GoogleAuthenticateSettings Google { get; set; }
}
