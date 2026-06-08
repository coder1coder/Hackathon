import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogTitle, MatDialogContent, MatDialogClose } from '@angular/material/dialog';
import { MatFormField, MatInput } from '@angular/material/input';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { FormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';

@Component({
    selector: 'cancel-join-request-comment-dialog',
    templateUrl: 'cancelJoinRequestCommentDialog.component.html',
    imports: [MatDialogTitle, MatDialogContent, MatFormField, MatInput, CdkTextareaAutosize, FormsModule, MatButton, MatDialogClose]
})
export class CancelJoinRequestCommentDialogComponent {
  dialogRef = inject<MatDialogRef<CancelJoinRequestCommentDialogComponent>>(MatDialogRef);
  data = inject(MAT_DIALOG_DATA);

  onNoClick(): void {
    this.dialogRef.close();
  }
}
