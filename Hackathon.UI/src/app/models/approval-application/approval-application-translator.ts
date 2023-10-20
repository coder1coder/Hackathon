import { ApprovalApplicationStatusEnum } from "./approval-application-status.enum";

export class ApprovalApplicationTranslator {
  public static GetName = (e: ApprovalApplicationStatusEnum): string => ApprovalApplicationStatusEnum[e].toLowerCase();
  public static Translate = (e: ApprovalApplicationStatusEnum): string => {
    switch (e) {
      case ApprovalApplicationStatusEnum.Requested: return 'Запрос отправлен';
      case ApprovalApplicationStatusEnum.Approved: return 'Запрос одобрен';
      case ApprovalApplicationStatusEnum.Rejected: return 'Запорос отклонён';

      default: return ApprovalApplicationStatusEnum[e];
    }
  }
}
