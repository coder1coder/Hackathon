import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'cancel-join-request-comment-dialog',
    templateUrl: 'cancelJoinRequestCommentDialog.component.html',
    standalone: false
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
