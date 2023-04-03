import {BaseChatMessage} from "./BaseChatMessage";

export class TeamChatMessage extends BaseChatMessage
{
  teamId:number

  constructor(ownerId:number, message:string) {
    super(ChatMessageContext.TeamChat, ownerId, message);
  }
}

export enum ChatMessageContext
{
  TeamChat
}

export enum ChatMessageOption {
  Default = 0,
  WithNotify = 1
}
