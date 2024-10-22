namespace Hackathon.Configuration;

/// <summary>
/// Настройки отправки Email
/// </summary>
public sealed class EmailSettings
{
    /// <summary>
    /// Время жизни запроса на подтверждение Email в минутах
    /// </summary>
    public int EmailConfirmationRequestLifetime { get; set; } = 5;

    /// <summary>
    /// Настройки для отправки Email
    /// </summary>
    public EmailSenderSettings EmailSender { get; set; } = new();
}
