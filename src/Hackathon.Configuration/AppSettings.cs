using Hackathon.Configuration.IntegrationEvents;

namespace Hackathon.Configuration;

/// <summary>
/// Настройки приложения
/// </summary>
public sealed class AppSettings
{
    /// <summary>
    /// Настройки Origins
    /// </summary>
    public OriginsOptions OriginsOptions { get; set; }
    
    /// <summary>
    /// Включить расширенные логи источников данных
    /// </summary>
    public bool? EnableSensitiveDataLogging { get; set; }
    
    /// <summary>
    /// Настройки каналов real-time событий
    /// </summary>
    public HubsSettings Hubs { get; set; }
}
