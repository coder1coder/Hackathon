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

  public eventStatusLabelMapping = EventStatusLabelMapping;
  public statuses = Object.values(EventStatus);

  form = new FormGroup({
    status: new FormControl(this.statuses),
    message: new FormControl('')
  })

  constructor(
    public dialogRef: MatDialogRef<EventNewStatusDialog>,
    @Inject(MAT_DIALOG_DATA) private dialogData: any) {
     // this.statuses = dialogData.statuses;
      this.statuses = Object.values(EventStatus).filter(value => typeof value === 'string');
      console.log('this.statuses: ', this.statuses);
  }

  ngAfterViewInit(): void {
  }

  createStatus() {

    //this.statuses = Object.keys(EventStatus).filter(value => typeof value === 'number');

    //this.statuses = Object.keys(EventStatus).filter((key, value) => !isNaN(Number(EventStatus[value])));

    let createTeamModel = new ChangeEventStatusMessage();
    createTeamModel.status =  this.form.get('status')?.value;
    createTeamModel.message =  this.form.get('message')?.value;

    this.dialogRef.close(createTeamModel);
  }

  getEnumValue(status: string) : string {
    for(let key in EventStatus) if (EventStatus[key] === status) return key;
    return status!;
  }
}
