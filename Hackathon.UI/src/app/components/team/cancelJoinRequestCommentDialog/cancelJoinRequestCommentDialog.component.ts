import {Component, Inject} from "@angular/core";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'cancel-join-request-comment-dialog',
  templateUrl: 'cancelJoinRequestCommentDialog.component.html',
})
export class CancelJoinRequestCommentDialog {
  constructor(
    public dialogRef: MatDialogRef<CancelJoinRequestCommentDialog>,
    @Inject(MAT_DIALOG_DATA) public data: string,
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}
