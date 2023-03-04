namespace Hackathon.Common.Configuration;

public class AppSettings
{
    public AuthOptions AuthOptions { get; set; }
    public OriginsOptions OriginsOptions { get; set; }
    public S3Options S3Options { get; set; }
    public bool? EnableSensitiveDataLogging { get; set; }
    public string PathBase { get; set; }
    public HubsSettings Hubs { get; set; }
    public MessageBrokerSettings MessageBrokerSettings { get; set; }

    /// <summary>
    /// Время жизни запроса на подтверждение Email в минутах
    /// </summary>
    public int EmailConfirmationRequestLifetime { get; set; } = 5;

    /// <summary>
    /// Настройки для отправки Email
    /// </summary>
    public EmailSenderSettings EmailSender { get; set; } = new();
}
