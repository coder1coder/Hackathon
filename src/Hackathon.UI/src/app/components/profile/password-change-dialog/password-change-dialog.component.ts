import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IUpdatePasswordParameters } from 'src/app/models/User/IUpdatePasswordParameters';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose } from '@angular/material/dialog';
import { MatFormField, MatLabel, MatInput } from '@angular/material/input';
import { MatButton } from '@angular/material/button';

@Component({
    selector: 'password-change-dialog',
    templateUrl: './password-change-dialog.component.html',
    imports: [MatDialogTitle, MatDialogContent, FormsModule, ReactiveFormsModule, MatFormField, MatLabel, MatInput, MatDialogActions, MatButton, MatDialogClose]
})
export class PasswordChangeDialogComponent implements OnInit {
  private formBuilder = inject(FormBuilder);
  dialogRef = inject<MatDialogRef<IUpdatePasswordParameters>>(MatDialogRef);
  dialogData = inject<IUpdatePasswordParameters>(MAT_DIALOG_DATA);

  public form!: FormGroup;

  ngOnInit(): void {
    this.initForm();
  }

  public confirm(): void {
    const parameters: IUpdatePasswordParameters = {
      currentPassword: this.form.get('currentPassword')?.value,
      newPassword: this.form.get('newPassword')?.value,
    };

    this.dialogRef.close(parameters);
  }

  private initForm(): void {
    this.form = this.formBuilder.group({
      currentPassword: new FormControl(this.dialogData?.currentPassword, [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(20),
      ]),
      newPassword: new FormControl(this.dialogData?.newPassword, [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(20),
      ]),
    });
  }
}
