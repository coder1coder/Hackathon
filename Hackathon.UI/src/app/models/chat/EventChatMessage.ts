import {BaseChatMessage} from "./BaseChatMessage";
import {ChatMessageContext, ChatMessageOption} from "./TeamChatMessage";

export class EventChatMessage extends BaseChatMessage
{
  eventId:number

  constructor(eventId: number, ownerId: number, message: string, options: ChatMessageOption = ChatMessageOption.Default) {
    super(ChatMessageContext.EventChat, ownerId, message, options);
    this.eventId = eventId;
  }
}
