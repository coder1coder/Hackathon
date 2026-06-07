import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose } from '@angular/material/dialog';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustomErrorStateMatcher } from '../../../common/functions/custom-error-state-matcher';
import { IProjectUpdateFromGitBranch } from '../../../models/Project/IProjectUpdateFromGitBranch';
import { CdkScrollable } from '@angular/cdk/scrolling';
import { MatFormField, MatLabel, MatInput } from '@angular/material/input';
import { MatButton } from '@angular/material/button';

@Component({
    selector: 'project-git-dialog',
    templateUrl: './project-git-dialog.component.html',
    imports: [MatDialogTitle, CdkScrollable, MatDialogContent, FormsModule, ReactiveFormsModule, MatFormField, MatLabel, MatInput, MatDialogActions, MatButton, MatDialogClose]
})
export class ProjectGitDialogComponent implements OnInit {
  public form!: FormGroup;
  matcher = new CustomErrorStateMatcher();

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<ProjectGitDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: IProjectUpdateFromGitBranch,
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  public confirm(): void {
    let link: string = this.form.get('linkToGitBranch')?.value;

    if (link?.length == 0) {
      link = '';
    }

    const parameters: IProjectUpdateFromGitBranch = {
      eventId: this.form.get('eventId')?.value ?? 0,
      teamId: this.form.get('teamId')?.value ?? 0,
      linkToGitBranch: link,
    };

    this.dialogRef.close(parameters);
  }

  private initForm(): void {
    this.form = this.formBuilder.group({
      eventId: new FormControl(this.dialogData?.eventId),
      teamId: new FormControl(this.dialogData?.teamId),
      linkToGitBranch: new FormControl(this.dialogData?.linkToGitBranch),
    });
  }
}
