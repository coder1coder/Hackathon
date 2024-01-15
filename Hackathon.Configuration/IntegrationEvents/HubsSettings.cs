namespace Hackathon.Configuration.IntegrationEvents;

/// <summary>
/// Настройки каналов real-time событий
/// </summary>
public class HubsSettings
{
    /// <summary>
    /// Уведомления
    /// </summary>
    public string Notifications { get; set; }
    
    /// <summary>
    /// Чаты
    /// </summary>
    public string Chat { get; set; }
    
    /// <summary>
    /// Дружба
    /// </summary>
    public string Friendship { get; set; }
    
    /// <summary>
    /// Мероприятия
    /// </summary>
    public string Events { get; set; }
}
