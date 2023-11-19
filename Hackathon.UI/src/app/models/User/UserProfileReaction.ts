export enum UserProfileReaction
{
  None = 0,
  Like = 1,
  Heart = 2,
  Fire = 4
}

export class UserProfileReactionModel {
    type: UserProfileReaction | undefined;
    count: number | undefined;
}
