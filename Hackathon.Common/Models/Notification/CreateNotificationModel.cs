namespace Hackathon.Common.Models.Notification;

public class CreateNotificationModel<T> where T: class 
{
    public NotificationType Type { get; set; }
    public long UserId { get; set; }
    public long? OwnerId { get; set; }
    public T Data { get; set; }
}