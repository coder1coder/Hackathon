import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CustomErrorStateMatcher } from '../../../common/functions/custom-error-state-matcher';
import { IProject } from '../../../models/Project/IProject';
import { IProjectDialogData } from '../../../models/Project/project-dialog.interface';

@Component({
  selector: 'project-dialog',
  templateUrl: './project-dialog.component.html',
})
export class ProjectDialogComponent implements OnInit {
  public form: FormGroup;
  matcher = new CustomErrorStateMatcher();

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<ProjectDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: IProjectDialogData,
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  public confirm(): void {
    const project: IProject = {
      name: this.form.get('name')?.value,
      description: this.form.get('description')?.value,
      eventId: this.form.get('eventId')?.value ?? 0,
      teamId: this.form.get('teamId')?.value ?? 0,
    };

    this.dialogRef.close(project);
  }

  private initForm(): void {
    this.form = this.formBuilder.group({
      name: new FormControl(this.dialogData?.project?.name, [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(100),
      ]),
      description: new FormControl(this.dialogData?.project?.description),
      eventId: new FormControl(this.dialogData?.project?.eventId),
      teamId: new FormControl(this.dialogData?.project?.teamId),
    });
  }
}
