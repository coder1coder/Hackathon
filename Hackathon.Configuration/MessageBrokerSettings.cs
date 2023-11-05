namespace Hackathon.Configuration;

/// <summary>
/// Настройки брокера сообщений
/// </summary>
public class MessageBrokerSettings
{
    /// <summary>
    /// Хост
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
}
