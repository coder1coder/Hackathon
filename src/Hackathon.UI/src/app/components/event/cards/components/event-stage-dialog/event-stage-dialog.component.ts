import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose } from '@angular/material/dialog';
import { EventStage } from '../../../../../models/Event/EventStage';
import { CustomErrorStateMatcher } from '../../../../../common/functions/custom-error-state-matcher';
import { CdkScrollable } from '@angular/cdk/scrolling';
import { MatFormField, MatLabel, MatInput, MatError } from '@angular/material/input';

import { MatButton } from '@angular/material/button';

@Component({
    selector: 'event-stage-dialog',
    templateUrl: './event-stage-dialog.component.html',
    styleUrls: ['./event-stage-dialog.component.scss'],
    imports: [MatDialogTitle, CdkScrollable, MatDialogContent, FormsModule, ReactiveFormsModule, MatFormField, MatLabel, MatInput, MatError, MatDialogActions, MatButton, MatDialogClose]
})
export class EventStageDialogComponent implements OnInit {
  public form = new FormGroup({
    name: new FormControl(''),
    duration: new FormControl(0),
  });
  matcher = new CustomErrorStateMatcher();

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<EventStageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private dialogData: EventStageDialogData,
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.form = this.fb.group({
      name: new FormControl(this.dialogData?.eventStage?.name!, [
        Validators.required,
        this.nameShouldBeUnique(),
      ]),
      duration: new FormControl(this.dialogData?.eventStage?.duration!, [
        Validators.required,
        Validators.min(1),
      ]),
    });
  }

  public confirm(): void {
    const eventStage: EventStage = new EventStage();
    //TODO: remove type assertion
    eventStage.name = this.form.get('name')?.value as string;
    eventStage.duration = this.form.get('duration')?.value as number;

    this.dialogRef.close(eventStage);
  }

  nameShouldBeUnique(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const filtered: EventStage[] = this.dialogData?.eventStages?.filter(
        (x) =>
          x !== this.dialogData.eventStage && x.name?.toLowerCase() == control.value?.toLowerCase(),
      );

      return filtered?.length > 0
        ? { nameShouldBeUnique: ['Этап с таким наименованием уже существует'] }
        : null;
    };
  }
}

export class EventStageDialogData {
  eventStages!: EventStage[];
  eventStage: EventStage | undefined;
}
