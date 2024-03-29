import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthConstants } from '../../services/auth.constants';
import { RouterService } from '../../services/router.service';
import { IGetTokenResponse } from '../../models/IGetTokenResponse';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private storage = sessionStorage;

  constructor(private router: RouterService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authJson: string = this.storage.getItem(AuthConstants.STORAGE_AUTH_KEY);

    if (authJson !== undefined) {
      const auth: IGetTokenResponse = JSON.parse(authJson);
      if (auth && auth.token) {
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${auth.token}`,
          },
        });
      }
    }
    return next.handle(request).pipe(
      tap(
        () => {},
        (err: any) => {
          if (err instanceof HttpErrorResponse) {
            if (err.status !== 401) return;
            this.router.Profile.Login();
          }
        },
      ),
    );
  }
}
