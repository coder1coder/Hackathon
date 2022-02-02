using Hackathon.Common.Configuration;

namespace Hackathon.Common.Configuration;

public class AppSettings
{
    public AuthOptions AuthOptions { get; set; }
    public AdministratorDefaults AdministratorDefaults { get; set; }
    public OriginsOptions OriginsOptions { get; set; }
    public bool? EnableSensitiveDataLogging { get; set; }
    public string PathBase { get; set; }
    public string MessageHubPrefix { get; set; }
}