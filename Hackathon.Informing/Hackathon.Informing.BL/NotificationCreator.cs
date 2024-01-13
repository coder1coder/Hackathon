using System;
using Hackathon.Informing.Abstractions.Models.Notifications;
using Hackathon.Informing.Abstractions.Models.Notifications.Data;

namespace Hackathon.Informing.BL;

public static class NotificationCreator
{
    public static CreateNotificationModel<SystemNotificationData> System(SystemNotificationData data, long userId, 
        long? ownerId = null)
        => Create(data, userId, ownerId);

    public static CreateNotificationModel<TeamJoinRequestDecisionData> TeamJoinRequestDecision(TeamJoinRequestDecisionData data, 
        long userId, long? ownerId = null)
        => Create(data, userId, ownerId);

    private static CreateNotificationModel<T> Create<T>(T data, long userId, long? ownerId = null) => new()
    {
        Type = data switch
        {
            SystemNotificationData => NotificationType.System,
            TeamJoinRequestDecisionData => NotificationType.TeamJoinRequestDecision,
            _ => throw new AggregateException("Не удалось создать уведомление. Для указанного набора данных не определен тип.")
        },
        Data = data,
        UserId = userId,
        OwnerId = ownerId
    };
}
