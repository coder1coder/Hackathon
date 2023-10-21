import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from "./components/login/login.component";
import { RegisterComponent } from "./components/register/register.component";
import { NotFoundComponent } from "./components/not-found/not-found.component";
import { AuthGuard } from "./services/auth.guard";
import { EventListComponent } from "./components/event/list/event.list.component";
import { UserListComponent } from "./components/user/list/user.list.component";
import { TeamListComponent } from "./components/team/list/team.list.component";
import { TeamNewComponent } from "./components/team/new/team.new.component";
import { NotificationListComponent } from "./components/notification/list/notification.list.component";
import { ProfilePageForLoggedUsersGuard } from "./services/profile-page-for-logged-users-guard.service";
import { TeamViewComponent } from "./components/team/view/team.view.component";
import { ProfileViewComponent } from "./components/profile/view/profile.view.component";
import { EventLogComponent } from "./components/eventlog/eventLog.list.component";
import { UserTeamComponent } from "./components/team/userTeam/userTeam.component";
import { EventCardFactoryComponent } from "./components/event/cards/event-card-factory.component";
import { EventCreateEditCardComponent } from "./components/event/cards/event-create-edit-card/event-create-edit-card.component";
import { EventCardGuard } from "./services/guards/event-card.guard";
import {
  ApprovalApplicationListComponent
} from "./components/approval-application/list/approval-application-list.component";

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full'},
  { path: 'login', component: LoginComponent, canActivate: [ProfilePageForLoggedUsersGuard] },
  { path: 'logout', component: LoginComponent },
  { path: 'register', component: RegisterComponent , canActivate: [ProfilePageForLoggedUsersGuard]},

  { path: 'profile', component: ProfileViewComponent, canActivate: [AuthGuard] },
  { path: 'events/new', component: EventCreateEditCardComponent, canActivate: [AuthGuard] },
  { path: 'events/edit/:eventId', component: EventCreateEditCardComponent, canActivate: [AuthGuard] },
  { path: 'events/:eventId', component: EventCardFactoryComponent, canActivate: [AuthGuard, EventCardGuard] },
  { path: 'events', component: EventListComponent, canActivate: [AuthGuard] },

  { path: 'teams/new', component: TeamNewComponent, canActivate: [AuthGuard] },
  { path: 'teams', component: TeamListComponent, canActivate: [AuthGuard] },
  { path: 'team', component: UserTeamComponent, canActivate: [AuthGuard] },
  { path: 'team/:teamId', component: TeamViewComponent, canActivate: [AuthGuard] },

  { path: 'users', component: UserListComponent, canActivate: [AuthGuard] },
  { path: 'users/:userId', component: ProfileViewComponent, canActivate: [AuthGuard] },
  { path: 'notifications', component: NotificationListComponent, canActivate: [AuthGuard]},

  { path: 'approval-applications', component: ApprovalApplicationListComponent, canActivate: [AuthGuard] },

  { path: 'eventLog', component: EventLogComponent, canActivate: [AuthGuard] },

  { path: '**', component: NotFoundComponent, canActivate: [AuthGuard] },
  { path: 'not-found', component: NotFoundComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes,{ onSameUrlNavigation: 'reload' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
