namespace Hackathon.Informing.Abstractions.Models.Notifications.Data;

/// <summary>
/// Модель данных системного уведомления
/// </summary>
public class SystemNotificationData
{
    /// <summary>
    /// Сообщение
    /// </summary>
    public string Message { get; private set; }
    
    public SystemNotificationData(string message)
    {
        Message = message;
    }
}
