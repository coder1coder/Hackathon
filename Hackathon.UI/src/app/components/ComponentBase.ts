import { Injectable, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Subject } from 'rxjs';

@Injectable()
export abstract class ComponentBase implements OnDestroy {
  public abstract form: FormGroup;
  public validators = Validators;

  public getFormControl(controlName: string, defValue?: any, validators?: ValidatorFn[]): FormControl {
    if (!this.form.contains(controlName)) {
      this.form.addControl(controlName, new FormControl(defValue, validators));
    }
    return this.form.get(controlName) as FormControl;
  }

  public resetControls(...names: Array<string>) {
    names.forEach((controlName) => {
      this.getFormControl(controlName).reset()
    })
  }

  protected destroy$ = new Subject();

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }
}
