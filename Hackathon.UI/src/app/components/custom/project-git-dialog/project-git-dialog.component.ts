import {Component, Inject, OnInit} from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import {FormBuilder, FormControl, FormGroup} from "@angular/forms";
import {CustomErrorStateMatcher} from "../../../common/functions/custom-error-state-matcher";
import {IProjectUpdateFromGitBranch} from "../../../models/Project/IProjectUpdateFromGitBranch";

@Component({
  selector: 'project-git-dialog',
  templateUrl: './project-git-dialog.component.html'
})
export class ProjectGitDialog implements OnInit {

  public form: FormGroup;
  matcher = new CustomErrorStateMatcher();

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<ProjectGitDialog>,
    @Inject(MAT_DIALOG_DATA) public dialogData: IProjectUpdateFromGitBranch
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  confirm() {
    let link = this.form.get('linkToGitBranch')?.value;

    if (link !== 'undefined' && link.length === 0)
    {
      link = null;
    }

    let parameters: IProjectUpdateFromGitBranch = {
      eventId: this.form.get('eventId')?.value ?? 0,
      teamId: this.form.get('teamId')?.value ?? 0,
      linkToGitBranch: link
    };

    this.dialogRef.close(parameters);
  }

  private initForm() {
    this.form = this.formBuilder.group({
      eventId: new FormControl(this.dialogData?.eventId),
      teamId: new FormControl(this.dialogData?.teamId),
      linkToGitBranch: new FormControl(this.dialogData?.linkToGitBranch),
    })
  }
}
