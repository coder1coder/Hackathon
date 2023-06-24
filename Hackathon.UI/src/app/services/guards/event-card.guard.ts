import {Injectable} from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  NavigationEnd,
  Router,
  RouterStateSnapshot,
  UrlTree
} from '@angular/router';
import {filter, Observable, Subject, takeUntil} from 'rxjs';
import {EventService} from "../event/event.service";
import {AuthService} from "../auth.service";
import {RouterService} from "../router.service";
import {EventHttpService} from "../event/event.http-service";
import {map} from "rxjs/operators";
import {SnackService} from "../snack.service";

@Injectable({
  providedIn: 'root'
})
export class EventCardGuard implements CanActivate {

  private destroy$: Subject<boolean> = new Subject<boolean>();
  private previousUrl: string;
  private currentUrl: string;

  constructor(
    private authService: AuthService,
    private eventService: EventService,
    private eventClient: EventHttpService,
    private routerService: RouterService,
    private router: Router,
    private snackService: SnackService) {

    this.currentUrl = this.router.url;

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.previousUrl = this.currentUrl;
        this.currentUrl = event.url;
      }
    });

  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

      if (!this.authService.isLoggedIn()) {
        this.routerService.Profile.Login();
        return false;
      }

      let eventId = Number(route.paramMap.get('eventId'));
      if (isNaN(eventId))
        return false;

      return this.eventClient
        .getById(eventId)
        .pipe(
          takeUntil(this.destroy$),
          filter((event) => !!event),
          map((event)=>
          {
            let canView = this.eventService.canView(event);

            if (!canView)
            {
              this.snackService.open(`Нет доступа на мероприятие`)

              if (this.currentUrl == '/')
                this.routerService.Homepage();

            }

            return canView;
          }));
  }
}
