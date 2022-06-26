namespace Hackathon.Common.Models.Notification;

public class CreateNotificationModel<T> where T: class 
{
    public NotificationType Type { get; set; }
    public long UserId { get; set; }
    public long? OwnerId { get; set; }
    public T Data { get; set; }
}

public static class CreateNotificationModel
{
    public static CreateNotificationModel<InfoNotificationData> Information(long userId, string message, long? ownerId = null)
        => new()
        {
            Type = NotificationType.Information,
            OwnerId = ownerId,
            UserId = userId,
            Data = new InfoNotificationData
            {
                Message = message
            }
        };
}