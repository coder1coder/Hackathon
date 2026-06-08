import { Component, Inject } from '@angular/core';
import { WithFormBaseComponent } from '../../../common/base-components/with-form-base.component';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose } from '@angular/material/dialog';
import { IApprovalApplication } from '../../../models/approval-application/approval-application.interface';
import { CdkScrollable } from '@angular/cdk/scrolling';
import { MatFormField, MatLabel, MatInput, MatError } from '@angular/material/input';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';

import { MatButton } from '@angular/material/button';

@Component({
    selector: 'app-approval-application-reject-modal',
    templateUrl: './approval-application-reject-modal.component.html',
    styleUrls: ['./approval-application-reject-modal.component.scss'],
    imports: [MatDialogTitle, CdkScrollable, MatDialogContent, FormsModule, ReactiveFormsModule, MatFormField, MatLabel, MatInput, CdkTextareaAutosize, MatError, MatDialogActions, MatButton, MatDialogClose]
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
