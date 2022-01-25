import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ChangeEventStatusMessage } from 'src/app/models/Event/ChangeEventStatusMessage';
import { EventStatus } from 'src/app/models/EventStatus';

@Component({
  selector: 'app-event-new-status-dialog',
  templateUrl: './event-new-status-dialog.component.html',
  styleUrls: ['./event-new-status-dialog.component.scss']
})

export class EventNewStatusDialogComponent {

  statuses: EventStatus[];
  selectedStatusValue!: number;

  form = new FormGroup({
    status: new FormControl(),
    message: new FormControl()
  })

  constructor(
    public dialogRef: MatDialogRef<EventNewStatusDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private dialogData: any) {
     this.statuses = this.dialogData.statuses;

     if (this.statuses?.length > 0)
       this.selectedStatusValue = this.statuses[0];
  }

  createStatus() {
    this.dialogRef.close(new ChangeEventStatusMessage(
      this.selectedStatusValue,
      this.form.get('message')?.value)
    );
  }

  getEventStatusName(status:EventStatus){
    return EventStatus[status];
  }
}
