import { Router } from '@angular/router';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class RouterService {
  constructor(
    public Events: EventsRouter,
    public Teams: TeamsRouter,
    public Users: UsersRouter,
    public Profile: ProfileRouter,
    public Notifications: NotificationsRouter,
    public Error: ErrorRouter,
    private router: Router,
  ) {}
  Homepage = (): Promise<boolean> => this.router.navigate([``]);
}

@Injectable({ providedIn: 'root' })
export class EventsRouter {
  constructor(private router: Router) {}
  List = (): Promise<boolean> => this.router.navigate([`events`]);
  View = (eventId: number): Promise<boolean> => this.router.navigate([`events/${eventId}`]);
  New = (): Promise<boolean> => this.router.navigate([`events/new`]);
  Edit = (eventId: number): Promise<boolean> => this.router.navigate([`events/edit/${eventId}`]);
}

@Injectable({ providedIn: 'root' })
export class TeamsRouter {
  constructor(private router: Router) {}
  New = (eventId?: number): Promise<boolean> =>
    this.router.navigate(['/teams/new'], { queryParams: { eventId: eventId } });
  View = (teamId: number): Promise<boolean> => this.router.navigateByUrl(`team/${teamId}`);
  List = (): Promise<boolean> => this.router.navigate([`teams`]);
  MyTeam = (): Promise<boolean> => this.router.navigate(['team']);
}

@Injectable({ providedIn: 'root' })
export class UsersRouter {
  constructor(private router: Router) {}
  View = (userId: number): Promise<boolean> => this.router.navigateByUrl(`users/${userId}`);
}

@Injectable({ providedIn: 'root' })
export class ProfileRouter {
  constructor(private router: Router) {}
  View = (): Promise<boolean> => this.router.navigate(['profile']);
  Login = (): Promise<boolean> => this.router.navigate(['login']);
  Logout = (): Promise<boolean> => this.router.navigate(['logout']);
  Register = (): Promise<boolean> => this.router.navigate(['/register']);
}

@Injectable({ providedIn: 'root' })
export class NotificationsRouter {
  constructor(private router: Router) {}
  List = (): Promise<boolean> => this.router.navigate(['notifications']);
}

@Injectable({ providedIn: 'root' })
export class ApprovalApplicationsRouter {
  constructor(private router: Router) {}
  List = (): Promise<boolean> => this.router.navigate(['approval-applications']);
}

@Injectable({ providedIn: 'root' })
export class ErrorRouter {
  constructor(private router: Router) {}
  NotFound = (): Promise<boolean> => this.router.navigate(['not-found']);
}
