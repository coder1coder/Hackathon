import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginComponent} from "./components/login/login.component";
import {RegisterComponent} from "./components/register/register.component";
import {NotFoundComponent} from "./components/not-found/not-found.component";
import {AuthGuard} from "./services/auth.guard";
import {ProfileComponent} from "./components/profile/profile.component";
import {EventListComponent} from "./components/event/list/event.list.component";
import {EventViewComponent} from "./components/event/view/event.view.component";
import {EventNewComponent} from "./components/event/new/event.new.component";
import {UserListComponent} from "./components/user/list/user.list.component";
import {UserViewComponent} from "./components/user/view/user.view.component";
import {TeamListComponent} from "./components/team/list/team.list.component";
import {TeamNewComponent} from "./components/team/new/team.new.component";
import {RedirectService} from "./services/redirect.service";


const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full'},
  { path: 'login', component: LoginComponent, canActivate: [RedirectService] },
  { path: 'logout', component: LoginComponent },
  { path: 'register', component: RegisterComponent , canActivate: [RedirectService]},

  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'events/new', component: EventNewComponent, canActivate: [AuthGuard] },
  { path: 'events/:eventId', component: EventViewComponent, canActivate: [AuthGuard] },
  { path: 'events', component: EventListComponent, canActivate: [AuthGuard] },
  // { path: 'team/:eventId', component: TeamComponent, canActivate: [AuthGuard] },
  { path: 'teams/new', component: TeamNewComponent, canActivate: [AuthGuard] },
  { path: 'teams', component: TeamListComponent, canActivate: [AuthGuard] },
  { path: 'users', component: UserListComponent, canActivate: [AuthGuard] },
  { path: 'users/:userId', component: UserViewComponent, canActivate: [AuthGuard] },
  { path: '**', component: NotFoundComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
