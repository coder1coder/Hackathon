namespace Hackathon.Common.Configuration;

public class AppSettings
{
    public OriginsOptions OriginsOptions { get; set; }
    public S3Options S3Options { get; set; }
    public bool? EnableSensitiveDataLogging { get; set; }
    public HubsSettings Hubs { get; set; }
    public MessageBrokerSettings MessageBrokerSettings { get; set; }
}
