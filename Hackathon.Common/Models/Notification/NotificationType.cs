using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hackathon.Common.Models.Notification;

/// <summary>
/// Тип уведомления
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// Системное информационное сообщение
    /// </summary>
    [NotificationGroup(NotificationGroup.System)]
    System = 0,

    /// <summary>
    /// Приглашение в команду
    /// </summary>
    [NotificationGroup(NotificationGroup.Teams)]
    TeamInvite = 1,

    /// <summary>
    /// Приглашение на мероприятие
    /// </summary>
    [NotificationGroup(NotificationGroup.Events)]
    EventInvite = 2,

    /// <summary>
    /// Принятие в команду
    /// </summary>
    [NotificationGroup(NotificationGroup.Teams)]
    TeamAcceptance = 3,

    /// <summary>
    /// Отклонение запроса на присоединения к команде
    /// </summary>
    [NotificationGroup(NotificationGroup.Teams)]
    JoinTeamRejection = 4,

    /// <summary>
    /// Запрос дружбы
    /// </summary>
    [NotificationGroup(NotificationGroup.Friends)]
    FriendshipRequest = 5,

    /// <summary>
    /// Запрос дружбы принят
    /// </summary>
    [NotificationGroup(NotificationGroup.Friends)]
    FriendshipAccepted = 6,

    /// <summary>
    /// Запрос дружбы отклонен
    /// </summary>
    [NotificationGroup(NotificationGroup.Friends)]
    FriendshipRejected = 7,

    /// <summary>
    /// Мероприятие скоро начнется
    /// </summary>
    [NotificationGroup(NotificationGroup.Events)]
    EventWillStartSoon = 8,

    /// <summary>
    /// Новое сообщение в чате команды
    /// </summary>
    [NotificationGroup(NotificationGroup.Teams)]
    NewTeamChatMessage = 9,

    /// <summary>
    /// Новое сообщение в чате мероприятия
    /// </summary>
    [NotificationGroup(NotificationGroup.Events)]
    NewEventChatMessage = 10,
}

public class NotificationGroupAttribute: Attribute
{
    public NotificationGroup Group { get; }

    public NotificationGroupAttribute(NotificationGroup notificationGroup)
    {
        Group = notificationGroup;
    }
}

public static class NotificationTypeExtensions
{
    public static NotificationGroup ToNotificationGroup(this NotificationType type)
        => type
            .GetType()
            .GetField(type.ToString())
            ?.GetCustomAttribute<NotificationGroupAttribute>(false)
            ?.Group ?? NotificationGroup.System;

    public static IEnumerable<NotificationType> GetNotificationTypes(this NotificationGroup group)
        => typeof(NotificationType)
            .GetFields()
            .Select(x =>
            {
                var attr = x.GetCustomAttribute<NotificationGroupAttribute>(false);
                return attr?.Group != group
                    ? null
                    : x.GetValue(null);
            })
            .Where(x=>x is not null)
            .OfType<NotificationType>()
            .ToArray();
}
