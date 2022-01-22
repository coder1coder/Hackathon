import { AfterViewInit, Component, Inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ChangeEventStatusMessage } from 'src/app/models/Event/ChangeEventStatusMessage';
import { EventStatus, EventStatusLabelMapping } from 'src/app/models/EventStatus';

@Component({
  selector: 'event-status',
  templateUrl: './event-status.new.component.html',
  styleUrls: ['./event-status.new.component.scss']
})

export class EventNewStatusDialog implements AfterViewInit {

  eventStatusLabelMapping = EventStatusLabelMapping;
  statuses = Object.values(this.eventStatusLabelMapping);
  selectedOption!: EventStatus;

  form = new FormGroup({
    status: new FormControl(this.statuses),
    message: new FormControl('')
  })

  constructor(
    public dialogRef: MatDialogRef<EventNewStatusDialog>,
    @Inject(MAT_DIALOG_DATA) private dialogData: any) {
     this.statuses = this.dialogData.statuses;
  }

  ngAfterViewInit(): void {
  }

  createStatus() {
    let createTeamModel = new ChangeEventStatusMessage();
    createTeamModel.status =  this.selectedOption;
    createTeamModel.message =  this.form.get('message')?.value;

    this.dialogRef.close(createTeamModel);
  }

  getEnumValue(status: string) : string {
    for(let key in EventStatus)
    if (EventStatus[key] === status) {
      return EventStatus[key];
    }

    return status!;
  }
}
