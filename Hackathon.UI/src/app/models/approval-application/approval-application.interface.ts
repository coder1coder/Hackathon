import { ApprovalApplicationStatusEnum } from './approval-application-status.enum';
import { IUser } from '../User/IUser';
import { IEventListItem } from '../Event/IEventListItem';

export interface IApprovalApplication {
  id: number;
  applicationStatus: ApprovalApplicationStatusEnum;
  signerId?: number;
  signer: IUser;
  authorId: number;
  author: IUser;
  comment: string;
  requestedAt: Date;
  decisionAt: Date;
  event: IEventListItem;
}

export interface IApprovalApplicationFilter {
  status?: ApprovalApplicationStatusEnum;
}
