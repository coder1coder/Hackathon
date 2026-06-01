import { Component, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

@Component({
  selector: 'cancel-join-request-comment-dialog',
  templateUrl: 'cancelJoinRequestCommentDialog.component.html',
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
