import {Component, Inject, OnInit} from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {CustomErrorStateMatcher} from "../../../common/CustomErrorStateMatcher";
import {IProject} from "../../../models/Project/IProject";

@Component({
  selector: 'project-dialog',
  templateUrl: './project-dialog.component.html'
})
export class ProjectDialog implements OnInit {

  public form: FormGroup;
  matcher = new CustomErrorStateMatcher();

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<ProjectDialog>,
    @Inject(MAT_DIALOG_DATA) public dialogData: IProjectDialogData
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  confirm() {
    let project: IProject = {
      name: this.form.get('name')?.value,
      description: this.form.get('description')?.value,
      eventId: this.form.get('eventId')?.value ?? 0,
      teamId: this.form.get('teamId')?.value ?? 0,
    };

    this.dialogRef.close(project);
  }

  private initForm() {
    this.form = this.formBuilder.group({
      name: new FormControl(this.dialogData?.project?.name, [
        Validators.required,
      ]),
      description: new FormControl(this.dialogData?.project?.description),
      eventId: new FormControl(this.dialogData?.project?.eventId),
      teamId: new FormControl(this.dialogData?.project?.teamId),
    })
  }
}

export interface IProjectDialogData {
  project: IProject
}
