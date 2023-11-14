using Hackathon.Configuration.IntegrationEvents;

namespace Hackathon.Configuration;

public class AppSettings
{
    public OriginsOptions OriginsOptions { get; set; }
    public bool? EnableSensitiveDataLogging { get; set; }
    public HubsSettings Hubs { get; set; }
    public MessageBrokerSettings MessageBrokerSettings { get; set; }
}
