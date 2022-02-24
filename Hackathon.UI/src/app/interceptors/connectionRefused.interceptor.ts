import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import {Observable} from 'rxjs';
import {tap} from 'rxjs/operators';
import {AuthService} from "../services/auth.service";
import {RouterService} from "../services/router.service";

@Injectable()
export class ConnectionRefusedInterceptor implements HttpInterceptor {

  storage = sessionStorage;

  constructor(private router: RouterService, private authService: AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(request).pipe(
      tap(
        () => {},
        (err) => {
            if (err instanceof HttpErrorResponse && err.status === 0)
              this.router.Profile.Login().then(_=> this.authService.logout());
        }
      )
    )
  }
}
