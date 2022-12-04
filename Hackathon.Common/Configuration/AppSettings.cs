namespace Hackathon.Common.Configuration;

public class AppSettings
{
    public AuthOptions AuthOptions { get; set; }
    public AdministratorDefaults AdministratorDefaults { get; set; }
    public OriginsOptions OriginsOptions { get; set; }
    public S3Options S3Options { get; set; }
    public bool? EnableSensitiveDataLogging { get; set; }
    public string PathBase { get; set; }
    public HubsSettings Hubs { get; set; }
    public RabbitMqSettings RabbitMq { get; set; }
}

public class HubsSettings
{
    public string Notifications { get; set; }
    public string Chat { get; set; }
    public string Friendship { get; set; }
}

public class RabbitMqSettings
{
    public string Host { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
