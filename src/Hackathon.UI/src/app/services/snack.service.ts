import { Injectable } from '@angular/core';
import { MatLegacySnackBar as MatSnackBar, MatLegacySnackBarRef as MatSnackBarRef, LegacyTextOnlySnackBar as TextOnlySnackBar } from '@angular/material/legacy-snack-bar';
import { ActionsEnum } from '../common/emuns/actions.enum';

@Injectable({
  providedIn: 'root',
})
export class SnackService {
  constructor(private snackBar: MatSnackBar) {}

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
