import { Component, inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose } from '@angular/material/dialog';
import { ThemePalette } from '@angular/material/core';
import { MatButton } from '@angular/material/button';

@Component({
    selector: 'save-delete-modal',
    templateUrl: './custom-dialog.component.html',
    styleUrls: ['./custom-dialog.component.scss'],
    imports: [MatDialogTitle, MatDialogContent, MatDialogActions, MatButton, MatDialogClose]
})
export class CustomDialogComponent {
  dialogRef = inject<MatDialogRef<CustomDialogComponent>>(MatDialogRef);
  data = inject<ICustomDialogData>(MAT_DIALOG_DATA);
}

export interface ICustomDialogData {
  header?: string;
  content?: string;
  acceptButtonText?: string;
  acceptButtonColor?: ThemePalette;
  cancelButtonText?: string;
}
