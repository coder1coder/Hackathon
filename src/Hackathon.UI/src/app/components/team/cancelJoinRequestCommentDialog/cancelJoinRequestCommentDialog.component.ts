import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogTitle, MatDialogContent, MatDialogClose } from '@angular/material/dialog';
import { CdkScrollable } from '@angular/cdk/scrolling';
import { MatFormField, MatInput } from '@angular/material/input';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { FormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';

@Component({
    selector: 'cancel-join-request-comment-dialog',
    templateUrl: 'cancelJoinRequestCommentDialog.component.html',
    imports: [MatDialogTitle, CdkScrollable, MatDialogContent, MatFormField, MatInput, CdkTextareaAutosize, FormsModule, MatButton, MatDialogClose]
})
export class CancelJoinRequestCommentDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<CancelJoinRequestCommentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string,
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}
