import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {IUpdatePasswordParameters} from 'src/app/models/User/IUpdatePasswordParameters';

@Component({
  selector: 'password-change-dialog',
  templateUrl: './password-change-dialog.component.html',
})
export class PasswordChangeDialogComponent implements OnInit {
  public form: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<IUpdatePasswordParameters>,
    @Inject(MAT_DIALOG_DATA) public dialogData: IUpdatePasswordParameters,
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  public confirm(): void {
    const parameters: IUpdatePasswordParameters = {
      currentPassword: this.form.get('currentPassword')?.value,
      newPassword: this.form.get('newPassword')?.value
    };

    this.dialogRef.close(parameters);
  }

  private initForm(): void {
    this.form = this.formBuilder.group({
      currentPassword: new FormControl(this.dialogData?.currentPassword, [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(20)]
        ),
      newPassword: new FormControl(this.dialogData?.newPassword, [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(20)]),
    });
  }
}
