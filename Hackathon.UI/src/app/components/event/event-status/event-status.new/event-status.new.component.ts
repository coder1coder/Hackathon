import { AfterViewInit, Component, Inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ChangeEventStatusMessage } from 'src/app/models/Event/ChangeEventStatusMessage';
import { EventStatus } from 'src/app/models/EventStatus';

@Component({
  selector: 'event-status',
  templateUrl: './event-status.new.component.html',
  styleUrls: ['./event-status.new.component.scss']
})

export class EventNewStatusDialog implements AfterViewInit {

  statuses: EventStatus[];
  selectedStatusValue!: number;

  form = new FormGroup({
    status: new FormControl(),
    message: new FormControl()
  })

  constructor(
    public dialogRef: MatDialogRef<EventNewStatusDialog>,
    @Inject(MAT_DIALOG_DATA) private dialogData: any) {
     this.statuses = this.dialogData.statuses;

     if (this.statuses?.length > 0)
       this.selectedStatusValue = this.statuses[0];
  }

  ngAfterViewInit(): void {
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
