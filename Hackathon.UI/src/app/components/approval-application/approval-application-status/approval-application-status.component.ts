import { Component, Input } from '@angular/core';
import { ApprovalApplicationStatusEnum } from '../../../models/approval-application/approval-application-status.enum';
import { ApprovalApplicationTranslator } from '../../../models/approval-application/approval-application-translator';

@Component({
  selector: 'app-approval-application-status',
  templateUrl: './approval-application-status.component.html',
  styleUrls: ['./approval-application-status.component.scss'],
})
export class ApprovalApplicationStatusComponent {
  @Input() status: ApprovalApplicationStatusEnum;
  public approvalApplicationTranslator = ApprovalApplicationTranslator;
}
