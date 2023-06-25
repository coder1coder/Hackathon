import {ChatMessageContext, ChatMessageOption} from "./TeamChatMessage";
import * as moment from "moment/moment";

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

  protected constructor(context:ChatMessageContext, ownerId:number, message:string, options: ChatMessageOption = ChatMessageOption.Default) {
    this.context = context;
    this.ownerId = ownerId;
    this.message = message;
    this.options = options;

    this.timestamp = moment.utc().toISOString();
  }

}
