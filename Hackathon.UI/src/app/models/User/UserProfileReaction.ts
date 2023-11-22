export enum UserProfileReaction
{
  None = 0,
  Like = 1,
  Heart = 2,
  Fire = 4
}

export interface IUserProfileReaction {
    type: UserProfileReaction;
    count: number;
}
