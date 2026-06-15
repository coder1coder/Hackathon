import { Injectable, inject } from '@angular/core';
import { MatSnackBar, MatSnackBarRef, TextOnlySnackBar } from '@angular/material/snack-bar';
import { ActionsEnum } from '../common/enums/actions.enum';

@Injectable({
  providedIn: 'root',
})
export class SnackService {
  private snackBar = inject(MatSnackBar);

  public open(
    text: string,
    actions: ActionsEnum = ActionsEnum.OK,
    duration: number = 1500,
  ): MatSnackBarRef<TextOnlySnackBar> {
    return this.snackBar.open(text, actions, {
      duration: duration,
    });
  }
}
