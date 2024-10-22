import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ThemePalette } from '@angular/material/core/common-behaviors/color';

@Component({
  selector: 'save-delete-modal',
  templateUrl: './custom-dialog.component.html',
  styleUrls: ['./custom-dialog.component.scss'],
})
export class CustomDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<CustomDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ICustomDialogData,
  ) {}
}

export interface ICustomDialogData {
  header?: string;
  content?: string;
  acceptButtonText?: string;
  acceptButtonColor?: ThemePalette;
  cancelButtonText?: string;
}
