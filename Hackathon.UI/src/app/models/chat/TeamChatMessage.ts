import { BaseChatMessage } from './BaseChatMessage';

export class TeamChatMessage extends BaseChatMessage {
  public teamId: number;

  constructor(
    teamId: number,
    ownerId: number,
    message: string,
    options: ChatMessageOption = ChatMessageOption.Default,
  ) {
    super(ChatMessageContext.TeamChat, ownerId, message, options);
    this.teamId = teamId;
  }
}

export enum ChatMessageContext {
  TeamChat = 0,
  EventChat = 1,
}

export enum ChatMessageOption {
  Default = 0,
  WithNotify = 1,
}
