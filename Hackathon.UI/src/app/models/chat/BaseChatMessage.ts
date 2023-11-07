import {ChatMessageContext, ChatMessageOption} from "./TeamChatMessage";
import * as moment from "moment/moment";

export abstract class BaseChatMessage {
  public context: ChatMessageContext;
  public ownerId: number;
  public userId: number;
  public message: string;
  public ownerFullName: string;
  public userFullName: string;
  public options: ChatMessageOption;
  public timestamp: string;

  protected constructor(context:ChatMessageContext, ownerId:number, message:string, options: ChatMessageOption = ChatMessageOption.Default) {
    this.context = context;
    this.ownerId = ownerId;
    this.message = message;
    this.options = options;

    this.timestamp = moment.utc().toISOString();
  }
}
