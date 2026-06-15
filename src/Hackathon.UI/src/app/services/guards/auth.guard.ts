import { Injectable, inject } from '@angular/core';
import { UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../auth.service';
import { RouterService } from '../router.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard  {
  private authService = inject(AuthService);
  private router = inject(RouterService);



  canActivate(): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (!this.authService.isLoggedIn()) {
      this.router.Profile.Login();
      return false;
    }

    return true;
  }
}
