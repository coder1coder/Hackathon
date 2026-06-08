import { Router } from '@angular/router';
import { Injectable, inject } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class RouterService {
  Events = inject(EventsRouter);
  Teams = inject(TeamsRouter);
  Users = inject(UsersRouter);
  Profile = inject(ProfileRouter);
  Notifications = inject(NotificationsRouter);
  Error = inject(ErrorRouter);
  private router = inject(Router);


  Homepage = (): Promise<boolean> => this.router.navigate([``]);
}

@Injectable({ providedIn: 'root' })
export class EventsRouter {
  private router = inject(Router);


  List = (): Promise<boolean> => this.router.navigate([`events`]);
  View = (eventId: number): Promise<boolean> => this.router.navigate([`events/${eventId}`]);
  New = (): Promise<boolean> => this.router.navigate([`events/new`]);
  Edit = (eventId: number): Promise<boolean> => this.router.navigate([`events/edit/${eventId}`]);
}

@Injectable({ providedIn: 'root' })
export class TeamsRouter {
  private router = inject(Router);


  New = (eventId?: number): Promise<boolean> =>
    this.router.navigate(['/teams/new'], { queryParams: { eventId: eventId } });
  View = (teamId: number): Promise<boolean> => this.router.navigateByUrl(`team/${teamId}`);
  List = (): Promise<boolean> => this.router.navigate([`teams`]);
  MyTeam = (): Promise<boolean> => this.router.navigate(['team']);
}

@Injectable({ providedIn: 'root' })
export class UsersRouter {
  private router = inject(Router);


  View = (userId: number): Promise<boolean> => this.router.navigateByUrl(`users/${userId}`);
}

@Injectable({ providedIn: 'root' })
export class ProfileRouter {
  private router = inject(Router);


  View = (): Promise<boolean> => this.router.navigate(['profile']);
  Login = (): Promise<boolean> => this.router.navigate(['login']);
  Logout = (): Promise<boolean> => this.router.navigate(['logout']);
  Register = (): Promise<boolean> => this.router.navigate(['/register']);
}

@Injectable({ providedIn: 'root' })
export class NotificationsRouter {
  private router = inject(Router);


  List = (): Promise<boolean> => this.router.navigate(['notifications']);
}

@Injectable({ providedIn: 'root' })
export class ApprovalApplicationsRouter {
  private router = inject(Router);


  List = (): Promise<boolean> => this.router.navigate(['approval-applications']);
}

@Injectable({ providedIn: 'root' })
export class ErrorRouter {
  private router = inject(Router);


  NotFound = (): Promise<boolean> => this.router.navigate(['not-found']);
}
