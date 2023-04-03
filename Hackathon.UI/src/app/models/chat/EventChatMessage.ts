import {BaseChatMessage} from "./BaseChatMessage";

export class EventChatMessage extends BaseChatMessage
{
  eventId:number

  constructor(context:ChatMessageContext, ownerId:number, message:string) {
    super(context, ownerId, message);
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
