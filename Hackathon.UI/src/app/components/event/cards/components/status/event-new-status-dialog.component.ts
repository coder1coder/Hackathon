import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ChangeEventStatusMessage } from 'src/app/models/Event/ChangeEventStatusMessage';
import { EventStatusTranslator, EventStatus } from "../../../../../models/Event/EventStatus";

@Component({
  selector: 'app-event-new-status-dialog',
  templateUrl: './event-new-status-dialog.component.html',
  styleUrls: ['./event-new-status-dialog.component.scss'],
})

export class EventNewStatusDialogComponent implements OnInit {
  public statuses: EventStatus[] = [];
  public selectedStatusValue!: number;
  public EventStatusTranslator = EventStatusTranslator;
  public form = new FormGroup({});

  private editStatus?: ChangeEventStatusMessage;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<EventNewStatusDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private dialogData: any,
  ) {
  }

  ngOnInit(): void {
    this.initForm();

    this.statuses = this.dialogData.statuses;
    this.editStatus = this.dialogData.editStatus;

    if (this.statuses?.length > 0 && !this.editStatus) {
     this.selectedStatusValue = this.statuses[0];
    }

    if(this.editStatus) {
      this.form.patchValue({
        message: this.editStatus?.message
       });

      this.selectedStatusValue = this.editStatus?.status;
    }
  }

  public createStatus(): void {
    this.dialogRef.close(new ChangeEventStatusMessage(
      this.selectedStatusValue,
      this.form.get('message')?.value)
    );
  }

  private initForm(): void {
    this.form = this.fb.group({
      status: [null],
      message: [null]
    })
  }
}


