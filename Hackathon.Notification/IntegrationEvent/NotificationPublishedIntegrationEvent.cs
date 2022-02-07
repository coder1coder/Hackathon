namespace Hackathon.Notification.IntegrationEvent;

public class NotificationPublishedIntegrationEvent: IIntegrationEvent
{
    public string Message = nameof(Message);
}