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
import {Router} from '@angular/router';
import {AuthService} from "../services/auth.service";

@Injectable()
export class ConnectionRefusedInterceptor implements HttpInterceptor {

  storage = sessionStorage;

  constructor(private router: Router, private authService: AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(request).pipe(
      tap(
        () => {},
        (err) => {
          if (err instanceof HttpErrorResponse) {
            if (err.status === 0)
            {
              this.authService.logout();
              this.router.navigate(['/login'])
            }

          }
        }
      )
    )
  }
}
