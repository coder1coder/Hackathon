import { Component, Inject, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {EventStage} from "../../../../../models/Event/EventStage";
import {CustomErrorStateMatcher} from "../../../../../common/CustomErrorStateMatcher";

@Component({
  selector: 'event-stage-dialog',
  templateUrl: './event-stage-dialog.component.html',
  styleUrls: ['./event-stage-dialog.component.scss']
})

export class EventStageDialogComponent implements OnInit {

  public form = new FormGroup({});
  matcher = new CustomErrorStateMatcher();

  constructor(private fb: FormBuilder,public dialogRef: MatDialogRef<EventStageDialogComponent>,
              @Inject(MAT_DIALOG_DATA) private dialogData: EventStageDialogData
  ) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {

    this.form = this.fb.group({
      name: new FormControl(this.dialogData?.eventStage?.name, [
        Validators.required,
        this.nameShouldBeUnique()
      ]),
      duration: new FormControl(this.dialogData?.eventStage?.duration, [
        Validators.required,
        Validators.min(1)
      ])
    })
  }

  public confirm():void{
    let eventStage = new EventStage();
    eventStage.name = this.form.get('name')?.value;
    eventStage.duration = this.form.get('duration')?.value;

    this.dialogRef.close(eventStage);
  }

  nameShouldBeUnique(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {

      let filtered = this.dialogData?.eventStages?.filter(x=>
        x !== this.dialogData.eventStage
        && x.name.toLowerCase() == control.value?.toLowerCase());

      return filtered?.length > 0
      ? {  'nameShouldBeUnique': ['Этап с таким наименованием уже существует'] }
      : null;
    };
  }
}

export class EventStageDialogData
{
  eventStages: EventStage[];
  eventStage: EventStage | undefined;
}
