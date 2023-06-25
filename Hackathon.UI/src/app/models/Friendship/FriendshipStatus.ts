export enum FriendshipStatus
{
  /**  Предлоджение дружбы отправлено */
  Pending = 1,

  /**  Предложение дружбы принято */
  Confirmed = 2,

  /**  Предложение дружбы отклонено */
  Rejected = 3
}

export interface IFriendship
{
  //Инициатор предложения дружбы
  proposerId: number;

  //Кому адресовано предложение дружбы
  userId: number;
  status: FriendshipStatus;
}

export enum GetOfferOption
{
  Any = 0,
  Outgoing = 1,
  Incoming = 2
}
