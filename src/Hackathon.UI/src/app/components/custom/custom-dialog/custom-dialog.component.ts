import { Component, Inject } from '@angular/core';
import { MatLegacyDialogRef as MatDialogRef, MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA } from '@angular/material/legacy-dialog';
import { ThemePalette } from '@angular/material/core';

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
