import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginComponent} from "./components/login/login.component";
import {RegisterComponent} from "./components/register/register.component";
import {NotFoundComponent} from "./components/not-found/not-found.component";
import {AuthGuard} from "./services/auth.guard";
import {EventListComponent} from "./components/event/list/event.list.component";
import {EventViewComponent} from "./components/event/view/event.view.component";
import {EventFormComponent} from "./components/event/form/event.form.component";
import {UserListComponent} from "./components/user/list/user.list.component";
import {UserViewComponent} from "./components/user/view/user.view.component";
import {TeamListComponent} from "./components/team/list/team.list.component";
import {TeamNewComponent} from "./components/team/new/team.new.component";
import {NotificationListComponent} from "./components/notification/list/notification.list.component";
import {RedirectService} from "./services/redirect.service";
import {UserTeamComponent} from "./components/team/user/userTeam.component";
import {TeamViewComponent} from "./components/team/view/team.view.component";
import {ProfileViewComponent} from "./components/profile/view/profile.view.component";
import {EventCardComponent} from "./components/event/card/event.card.component";


const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full'},
  { path: 'login', component: LoginComponent, canActivate: [RedirectService] },
  { path: 'logout', component: LoginComponent },
  { path: 'register', component: RegisterComponent , canActivate: [RedirectService]},

  { path: 'profile', component: ProfileViewComponent, canActivate: [AuthGuard] },
  { path: 'events/new', component: EventFormComponent, canActivate: [AuthGuard] },
  { path: 'events/edit/:eventId', component: EventFormComponent, canActivate: [AuthGuard] },
  { path: 'events/:eventId', component: EventCardComponent, canActivate: [AuthGuard] },
  { path: 'events', component: EventListComponent, canActivate: [AuthGuard] },

  { path: 'teams/new', component: TeamNewComponent, canActivate: [AuthGuard] },
  { path: 'teams', component: TeamListComponent, canActivate: [AuthGuard] },
  { path: 'team', component: UserTeamComponent, canActivate: [AuthGuard] },
  { path: 'team/:teamId', component: TeamViewComponent, canActivate: [AuthGuard] },

  { path: 'users', component: UserListComponent, canActivate: [AuthGuard] },
  { path: 'users/:userId', component: UserViewComponent, canActivate: [AuthGuard] },
  { path: 'notifications', component: NotificationListComponent, canActivate: [AuthGuard]},
  { path: '**', component: NotFoundComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes,{ onSameUrlNavigation: 'reload' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
