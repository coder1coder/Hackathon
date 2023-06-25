using System;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.Notification;

public static class NotificationFactory
{
    private static CreateNotificationModel<MessageNotificationData> MessageNotification(long? ownerId, long recipientId, string message, NotificationType type)
        => new()
        {
            Type = type,
            Data = new MessageNotificationData
            {
                Message = message
            },
            OwnerId = ownerId,
            UserId = recipientId
        };

    public static CreateNotificationModel<MessageNotificationData> EventStatusChanged(long recipientId, string message)
        => MessageNotification(null, recipientId, message, NotificationType.System);

    public static CreateNotificationModel<MessageNotificationData> YouHaveBeenAcceptedIntoTeam(long recipientId, string teamName)
        => MessageNotification(null, recipientId, $"Вы были приняты в команду '{teamName}'", NotificationType.TeamAcceptance);

    public static CreateNotificationModel<MessageNotificationData> JoinTeamRequestRejected(long recipientId, string teamName)
        => MessageNotification(null, recipientId, $"Запрос на вступление в команду '{teamName}' отклонен", NotificationType.JoinTeamRejection);

    public static CreateNotificationModel<MessageNotificationData> FriendshipRequest(UserModel fromUser, long recipientId)
        => MessageNotification(fromUser.Id, recipientId, $"Запрос дружбы от {fromUser}", NotificationType.FriendshipRequest);

    public static CreateNotificationModel<MessageNotificationData> FriendshipRequestAccepted(UserModel fromUser, long recipientId)
        => MessageNotification(fromUser.Id, recipientId, $"{fromUser} принял предложение дружбы", NotificationType.FriendshipAccepted);

    public static CreateNotificationModel<MessageNotificationData> FriendshipRequestRejected(UserModel fromUser, long recipientId)
        => MessageNotification(fromUser.Id, recipientId, $"{fromUser} отклонил предложение дружбы", NotificationType.FriendshipRejected);

    public static CreateNotificationModel<MessageNotificationData> EventWillStartSoon(long recipientId, string eventName)
        => MessageNotification(null, recipientId, $"Событие '{eventName}' скоро начнется", NotificationType.EventWillStartSoon);

    public static CreateNotificationModel<MessageNotificationData> NewChatMessage(long ownerId, long recipientId,
        string chatMessage, ChatMessageType chatType)
        => MessageNotification(ownerId, recipientId, $"Новое сообщение в чате: {chatMessage}",
            chatType switch
            {
                ChatMessageType.TeamChat => NotificationType.NewTeamChatMessage,
                ChatMessageType.EventChat => NotificationType.NewEventChatMessage,
                _ => throw new ArgumentOutOfRangeException(nameof(chatType), chatType, null)
            });
}
