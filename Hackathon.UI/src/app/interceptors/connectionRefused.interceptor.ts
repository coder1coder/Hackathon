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

@Injectable()
export class ConnectionRefusedInterceptor implements HttpInterceptor {

  storage = sessionStorage;

  constructor(private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(request).pipe(
      tap(
        () => {},
        (err) => {
          if (err instanceof HttpErrorResponse) {
            if (err.status === 0)
              this.router.navigate(['/login'])
          }
        }
      )
    )
  }
}
