import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import {tap} from 'rxjs/operators';
import {Router} from '@angular/router';
import {AuthConstants} from "../services/auth.constants";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  storage = sessionStorage;

  constructor(private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    let authJson = this.storage.getItem(AuthConstants.STORAGE_AUTH_KEY);

    if (authJson != undefined)
    {
      let auth = JSON.parse(authJson);

      if (auth && auth.token) {

        request = request.clone({
          setHeaders: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${auth.token}`
          }
        });
      }
    }

    return next.handle(request).pipe( tap(() => {},
      (err: any) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status !== 401) {
            return;
          }
          this.router.navigate(['login']);
        }
      }));
  }
}
