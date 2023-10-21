import { Component, EventEmitter, Input, Output } from '@angular/core';
import { WithFormBaseComponent } from "../../../common/base-components/with-form-base.component";
import { FormBuilder } from "@angular/forms";
import {
  IApprovalApplication,
  IApprovalApplicationFilter
} from "../../../models/approval-application/approval-application.interface";
import { ApprovalApplicationStatusEnum } from "../../../models/approval-application/approval-application-status.enum";

@Component({
  selector: 'app-approval-application-filter',
  templateUrl: './approval-application-filter.component.html',
  styleUrls: ['./approval-application-filter.component.scss'],
})
export class ApprovalApplicationFilterComponent extends WithFormBaseComponent {

  @Input() isFilterEnabled: boolean = true;
  @Input() approvalApplications: IApprovalApplication[] = [];
  @Output() changeFilterEmit: EventEmitter<IApprovalApplicationFilter> = new EventEmitter<IApprovalApplicationFilter>();

  public approvalApplicationStatuses = Object.keys(ApprovalApplicationStatusEnum)
    .map(x => parseInt(x)).
    filter(x => !isNaN(x));
  public form = this.fb.group({
    event: [null],
    status: [null],
  });

  constructor(
    private fb: FormBuilder,
  ) {
    super()
  }

  public submitFilter(): void {
    const filterData: IApprovalApplicationFilter = {
      eventId: this.getFormControl('event')?.value?.id ? this.getFormControl('event').value.id : null,
      status: (this.getFormControl('status')?.value !== null || this.getFormControl('status')?.value !== undefined) ? this.getFormControl('status').value : null,
    };
    this.changeFilterEmit.emit(filterData);
  }

  public clearFilter(): void {
    this.form.reset();
    this.changeFilterEmit.emit({
      eventId: null,
      status: null,
    });
  }
}
