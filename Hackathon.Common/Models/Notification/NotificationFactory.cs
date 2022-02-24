namespace Hackathon.Common.Models.Notification;

public static class NotificationFactory
{
    public static CreateNotificationModel<InfoNotificationData> InfoNotification(string message, long userId, long? ownerId = null)
    {
        return new CreateNotificationModel<InfoNotificationData>
        {
            Type = NotificationType.Information,
            Data = new InfoNotificationData
            {
                Message = message
            },
            OwnerId = ownerId,
            UserId = userId
        };
    }
}