import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginComponent} from "./components/login/login.component";
import {RegisterComponent} from "./components/register/register.component";
import {EventsComponent} from "./components/events/events.component";
import {NotFoundComponent} from "./components/not-found/not-found.component";
import {TeamsComponent} from "./components/teams/teams.component";
import {TeamComponent} from "./components/team/team.component";
import {AuthGuard} from "./services/auth.guard";
import {ProfileComponent} from "./components/profile/profile.component";
import {EventsNewComponent} from "./components/events/new/events.new.component";
import {EventsViewComponent} from "./components/events/view/events.view.component";

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'events/new', component: EventsNewComponent, canActivate: [AuthGuard] },
  { path: 'events/:eventId', component: EventsViewComponent, canActivate: [AuthGuard] },
  { path: 'events', component: EventsComponent, canActivate: [AuthGuard] },
  { path: 'team/:eventId', component: TeamComponent, canActivate: [AuthGuard] },
  { path: 'teams', component: TeamsComponent, canActivate: [AuthGuard] },
  { path: '**', component: NotFoundComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
