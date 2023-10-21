import { Injectable, NgZone } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthService } from "../services/auth.service";
import { RouterService } from "../services/router.service";
import { ErrorCodesEnum } from "../models/error-codes.enum";
import { ErrorProcessorService } from "../services/error-processor.service";

@Injectable()
export class ConnectionRefusedInterceptor implements HttpInterceptor {

  constructor(
    private router: RouterService,
    private authService: AuthService,
    private ngZone: NgZone,
    private errorProcessor: ErrorProcessorService,
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(request)
      .pipe(
      tap(
        () => {},
        (error) => {
          if (error instanceof HttpErrorResponse) {
            this.ngZone.run(() => {
              if (error.status === ErrorCodesEnum.NOT_CONNECTED) {
                this.router.Profile.Login().then(_=> this.authService.logout());
                this.errorProcessor.Process(error, 'Нет ответа от сервера. Проверьте соединение');
              } else if (error.status === ErrorCodesEnum.NOT_FOUND) {
                this.router.ErrorRouter.NotFound();
                this.errorProcessor.Process(error, 'Страница не найдена');
              } else if (error.status === ErrorCodesEnum.SERVER_ERROR) {
                this.errorProcessor.Process(error, 'Ошибка сервера. Попробуйте повторить запрос.');
              } else if (error.status === ErrorCodesEnum.UN_AUTHTORIZED) {
                this.router.Profile.Login().then(_=> this.authService.logout());
              }
            })
          }
        }
      )
    )
  }
}
