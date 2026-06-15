import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { ApprovalApplicationStatusEnum } from '../../../models/approval-application/approval-application-status.enum';
import { ApprovalApplicationTranslator } from '../../../models/approval-application/approval-application-translator';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-approval-application-status',
  templateUrl: './approval-application-status.component.html',
  styleUrls: ['./approval-application-status.component.scss'],
  standalone: true,
  imports: [MatIcon],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ApprovalApplicationStatusComponent {
  @Input() status!: ApprovalApplicationStatusEnum;
  public approvalApplicationTranslator = ApprovalApplicationTranslator;
}
