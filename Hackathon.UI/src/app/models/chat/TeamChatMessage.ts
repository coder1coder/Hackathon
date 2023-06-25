import {BaseChatMessage} from "./BaseChatMessage";

export class TeamChatMessage extends BaseChatMessage
{
  teamId:number

  constructor(teamId: number, ownerId:number, message:string, options: ChatMessageOption = ChatMessageOption.Default) {
    super(ChatMessageContext.TeamChat, ownerId, message, options);
    this.teamId = teamId;
  }
}

export enum ChatMessageContext
{
  TeamChat,
  EventChat
}

export enum ChatMessageOption {
  Default = 0,
  WithNotify = 1
}
