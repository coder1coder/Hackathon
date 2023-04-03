import {ChatMessageContext} from "./TeamChatMessage";
import {ChatMessageOption} from "./EventChatMessage";

export abstract class BaseChatMessage
{
  context!:ChatMessageContext;
  ownerId!:number;
  userId?:number;
  message!:string;
  ownerFullName!:string;
  userFullName!:string;
  options!:ChatMessageOption;
  timestamp!:string;

  protected constructor(context:ChatMessageContext, ownerId:number, message:string) {
    this.context = context;
    this.ownerId = ownerId;
    this.message = message;
  }

}
