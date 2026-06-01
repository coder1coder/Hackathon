import { Component, Inject } from '@angular/core';
import { WithFormBaseComponent } from '../../../common/base-components/with-form-base.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';
import { IApprovalApplication } from '../../../models/approval-application/approval-application.interface';

@Component({
  selector: 'app-approval-application-reject-modal',
  templateUrl: './approval-application-reject-modal.component.html',
  styleUrls: ['./approval-application-reject-modal.component.scss'],
})
export class ApprovalApplicationRejectModalComponent extends WithFormBaseComponent {
  public form: FormGroup = this.fb.group({
    comment: [null, [Validators.required]],
  });

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<ApprovalApplicationRejectModalComponent>,
    @Inject(MAT_DIALOG_DATA) private dialogData: IApprovalApplication,
  ) {
    super();
  }

  public get approvalApplication(): IApprovalApplication {
    return this.dialogData;
  }

  public confirm(): void {
    const comment: string = this.getFormControl('comment')?.value ?? null;
    this.dialogRef.close(comment);
  }
}
