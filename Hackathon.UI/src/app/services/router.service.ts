import {Router} from "@angular/router";
import {Injectable} from "@angular/core";

@Injectable({ providedIn: 'root' })
export class RouterService
{
  constructor(
    public Events: EventsRouter,
    public Teams:TeamsRouter,
    public Profile:ProfileRouter,
    public Notifications: NotificationsRouter) {
  }
}

@Injectable({ providedIn: 'root' })
export class EventsRouter
{
  constructor(protected router: Router) {}
  List = () => this.router.navigate([`events`])
  View = (eventId:number) => this.router.navigate([`events/${eventId}`])
  New = () => this.router.navigate([`events/new`])
  Edit = (eventId:number) => this.router.navigate([`events/edit/${eventId}`])
}

@Injectable({ providedIn: 'root' })
export class TeamsRouter
{
  constructor(protected router: Router) {}
  New = (eventId?:number) => this.router.navigate(["/teams/new"], { queryParams: { eventId: eventId } })
  View = (teamId:number) => this.router.navigateByUrl(`team/${teamId}`)
  List = () => this.router.navigate([`teams`])
}

@Injectable({ providedIn: 'root' })
export class ProfileRouter
{
  constructor(protected router: Router) {}
  View = () => this.router.navigate(['profile'])
  Login = () => this.router.navigate(['login'])
}

@Injectable({ providedIn: 'root' })
export class NotificationsRouter
{
  constructor(protected router: Router) {}
  List = () => this.router.navigate(['notifications'])
}
