namespace Hackathon.Informing.Abstractions.Models.Notifications;

public record CreateNotificationModel<T>(NotificationType Type, long UserId, long? OwnerId, T Data)
{
    public NotificationType Type { get; } = Type;
    public long UserId { get; } = UserId;
    public long? OwnerId { get;} = OwnerId;
    public T Data { get; } = Data;
}
