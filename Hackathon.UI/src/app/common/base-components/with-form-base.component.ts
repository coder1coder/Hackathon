import { Injectable, OnDestroy } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormControl,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Subject } from 'rxjs';

@Injectable()
export abstract class WithFormBaseComponent implements OnDestroy {
  public abstract form: FormGroup;
  public validators = Validators;

  protected destroy$ = new Subject();

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  getFormControl(controlName: string, defValue?: any, validators?: ValidatorFn[]): FormControl {
    if (!this.form.contains(controlName)) {
      this.form.addControl(controlName, new FormControl(defValue, validators));
    }
    return this.form.get(controlName) as FormControl;
  }

  getFormArray(controlName: string, defValue: any[] = [], validators?: ValidatorFn[]): FormArray {
    if (!this.form.contains(controlName)) {
      this.form.addControl(controlName, new FormArray(defValue, validators));
    }
    return this.form.get(controlName) as FormArray;
  }

  makeFormControl(control: AbstractControl): FormControl {
    return control as FormControl;
  }

  resetControls(...names: Array<string>): void {
    names.forEach((controlName) => {
      this.getFormControl(controlName).reset();
    });
  }
}
