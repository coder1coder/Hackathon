import { Injectable, inject } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthConstants } from '../../services/auth.constants';
import { RouterService } from '../../services/router.service';
import { IGetTokenResponse } from '../../models/IGetTokenResponse';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private router = inject(RouterService);

  private storage = sessionStorage;



  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authJson = this.storage.getItem(AuthConstants.STORAGE_AUTH_KEY);

    if (authJson) {
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
      // @ts-ignore
      catchError((err) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status !== 401) return;
          this.router.Profile.Login();
        }
        return of(err);
      }),
    );
  }
}
