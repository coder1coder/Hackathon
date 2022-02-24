import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import {RouterService} from "./router.service";

@Injectable({
  providedIn: 'root'
})
export class RedirectService implements CanActivate {

  constructor(private authService: AuthService, private router: RouterService) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

      if (this.authService.isLoggedIn()) {
        this.router.Profile.View();
        return false;
      }

      return true;
  }
}
