import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { WithFormBaseComponent } from '../../../common/base-components/with-form-base.component';
import { FormBuilder } from '@angular/forms';
import {
  IApprovalApplication,
  IApprovalApplicationFilter,
} from '../../../models/approval-application/approval-application.interface';
import { ApprovalApplicationStatusEnum } from '../../../models/approval-application/approval-application-status.enum';

@Component({
  selector: 'app-approval-application-filter',
  templateUrl: './approval-application-filter.component.html',
  styleUrls: ['./approval-application-filter.component.scss'],
})
export class ApprovalApplicationFilterComponent extends WithFormBaseComponent implements OnInit {
  @Input() isFilterEnabled: boolean = true;
  @Input() applyOnChange: boolean = false;
  @Input() approvalApplications: IApprovalApplication[] = [];
  @Output() onChanged: EventEmitter<IApprovalApplicationFilter> =
    new EventEmitter<IApprovalApplicationFilter>();

  public approvalApplicationStatuses = Object.keys(ApprovalApplicationStatusEnum)
    .map((statusEnum) => parseInt(statusEnum))
    .filter((statusEnum) => !isNaN(statusEnum));

  public form = this.fb.group({
    status: [ApprovalApplicationStatusEnum.Requested],
  });

  constructor(private fb: FormBuilder) {
    super();
  }

  public ngOnInit(): void {

    this.form.get('status').valueChanges.subscribe(value => {
      if (this.applyOnChange)
        this.applyFilter()
    });

    this.applyFilter();
  }

  public applyFilter(): void {
    const filterData: IApprovalApplicationFilter = {
      status:
        this.getFormControl('status')?.value !== null ||
        this.getFormControl('status')?.value !== undefined
          ? this.getFormControl('status').value
          : null,
    };
    this.onChanged.emit(filterData);
    this.form.markAsDirty();
  }

  public clearFilter(): void {
    this.form.reset();
    this.onChanged.emit({
      status: null,
    });
  }
}
