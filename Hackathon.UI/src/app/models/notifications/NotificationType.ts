export enum NotificationType {
  System = 0,
  TeamInvite = 1,
  EventInvite = 2,

  /* Принятие в команду */
  TeamAcceptance = 3,

  /* Отклонение запроса на присоединения к команде */
  JoinTeamRejection = 4,

  /* Запрос дружбы */
  FriendshipRequest = 5,

  /* Запрос дружбы принят */
  FriendshipAccepted = 6,

  /* Запрос дружбы отклонен */
  FriendshipRejected = 7,

  /* Мероприятие скоро начнется */
  EventWillStartSoon = 8,

  /* Новое сообщение в чате команды */
  NewTeamChatMessage = 9,

  /* Новое сообщение в чате мероприятия*/
  NewEventChatMessage = 10,
}
