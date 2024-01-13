namespace Hackathon.Informing.Abstractions.Models.Notifications;

public class CreateNotificationModel<T>
{
    public NotificationType Type { get; init; }
    public long UserId { get; init; }
    public long? OwnerId { get; init; }
    public T Data { get; init; }
}
