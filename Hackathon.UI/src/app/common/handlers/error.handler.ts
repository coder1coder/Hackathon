import { ErrorHandler, Injectable, NgZone } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from "../../services/auth.service";
import { RouterService } from "../../services/router.service";
import { ErrorCodesEnum } from "../../models/error-codes.enum";
import { ErrorProcessorService } from "../../services/error-processor.service";

@Injectable({
  providedIn: 'root'
})
export class GlobalErrorHandler implements ErrorHandler {

  constructor(
    private router: RouterService,
    private authService: AuthService,
    private ngZone: NgZone,
    private errorProcessor: ErrorProcessorService,
  ) {}

  handleError(error: Error | HttpErrorResponse): void {
    if (error instanceof HttpErrorResponse) {
      this.ngZone.run(() => {
        if (error.status === ErrorCodesEnum.NOT_CONNECTED) {
          this.router.Profile.Login().then(() => this.authService.logout());
          this.errorProcessor.Process(error, 'Нет ответа от сервера. Проверьте соединение');
        } else if (error.status === ErrorCodesEnum.SERVER_ERROR) {
          this.errorProcessor.Process(error, 'Ошибка сервера. Попробуйте повторить запрос.');
        } else if (error.status === ErrorCodesEnum.UN_AUTHTORIZED) {
          this.router.Profile.Login().then(() => this.authService.logout());
        } else if (error.status === ErrorCodesEnum.NOT_FOUND) {
          this.router.Error.NotFound();
        }
      });
    }
    console.error(error);
  }
}

export const errorHandler = {
  provide: ErrorHandler,
  useClass: GlobalErrorHandler,
};
