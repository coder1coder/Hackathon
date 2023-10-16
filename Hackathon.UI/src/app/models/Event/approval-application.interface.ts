import { ApprovalApplicationStatusEnum } from "./approval-application-status.enum";
import { IUser } from "../User/IUser";
import { IEventListItem } from "./IEventListItem";

export interface IApprovalApplication {
  Id: number;
  ApprovalApplicationStatus: ApprovalApplicationStatusEnum;
  SignerId?: number;
  Signer: IUser;
  AuthorId: number;
  Author: IUser;
  Comment: string;
  RequestedAt: Date;
  DecisionAt: Date;
  Event: IEventListItem;
}
