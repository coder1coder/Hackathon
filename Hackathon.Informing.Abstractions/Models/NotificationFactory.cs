namespace Hackathon.Informing.Abstractions.Models;

public static class NotificationFactory
{
    public static CreateNotificationModel<InfoNotificationData> InfoNotification(string message, long userId, long? ownerId = null)
        => new()
        {
            Type = NotificationType.System,
            Data = new InfoNotificationData
            {
                Message = message
            },
            OwnerId = ownerId,
            UserId = userId
        };
}
