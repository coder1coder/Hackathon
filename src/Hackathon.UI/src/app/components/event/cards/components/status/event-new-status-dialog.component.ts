import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose } from '@angular/material/dialog';
import { ChangeEventStatusMessage } from 'src/app/models/Event/ChangeEventStatusMessage';
import { EventStatusTranslator, EventStatus } from '../../../../../models/Event/EventStatus';
import { CdkScrollable } from '@angular/cdk/scrolling';
import { MatFormField, MatLabel, MatInput } from '@angular/material/input';
import { MatSelect, MatOption } from '@angular/material/select';
import { NgFor } from '@angular/common';
import { MatButton } from '@angular/material/button';

@Component({
    selector: 'app-event-new-status-dialog',
    templateUrl: './event-new-status-dialog.component.html',
    styleUrls: ['./event-new-status-dialog.component.scss'],
    imports: [MatDialogTitle, CdkScrollable, MatDialogContent, FormsModule, ReactiveFormsModule, MatFormField, MatLabel, MatSelect, NgFor, MatOption, MatInput, MatDialogActions, MatButton, MatDialogClose]
})
export class EventNewStatusDialogComponent implements OnInit {
  public statuses: EventStatus[] = [];
  public selectedStatusValue!: number;
  public EventStatusTranslator = EventStatusTranslator;
  public form = new FormGroup({
    status: new FormControl(''),
    message: new FormControl(''),
  });

  private editStatus?: ChangeEventStatusMessage;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<EventNewStatusDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private dialogData: any,
  ) {}

  ngOnInit(): void {
    this.initForm();

    this.statuses = this.dialogData.statuses;
    this.editStatus = this.dialogData.editStatus;

    if (this.statuses?.length > 0 && !this.editStatus) {
      this.selectedStatusValue = this.statuses[0];
    }

    if (this.editStatus) {
      this.form.patchValue({
        message: this.editStatus?.message,
      });

      this.selectedStatusValue = this.editStatus?.status;
    }
  }

  public createStatus(): void {
    this.dialogRef.close(
      //TODO: remove type assertion
      new ChangeEventStatusMessage(this.selectedStatusValue, this.form.get('message')?.value as string),
    );
  }

  private initForm(): void {
    this.form = new FormGroup({
      status: new FormControl(''),
      message: new FormControl(''),
    });
  }
}
