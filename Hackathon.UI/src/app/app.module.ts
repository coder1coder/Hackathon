import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from "./components/nav-menu/nav-menu.component";
import { LoginComponent } from "./components/login/login.component";
import { RegisterComponent } from "./components/register/register.component";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatInputModule } from "@angular/material/input";
import { MatIconModule } from "@angular/material/icon";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatButtonModule } from "@angular/material/button";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { MatListModule } from "@angular/material/list";
import { MatMenuModule } from "@angular/material/menu";
import { NotFoundComponent } from "./components/not-found/not-found.component";
import { MatTableModule } from "@angular/material/table";
import { MatPaginatorIntl, MatPaginatorModule } from "@angular/material/paginator";
import { Pagination } from "./common/Pagination";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { ConnectionRefusedInterceptor } from "./interceptors/connectionRefused.interceptor";
import { AuthInterceptor } from "./interceptors/auth.interceptor";
import { DefaultLayoutComponent } from "./components/layouts/default/default.layout.component";
import { MatToolbarModule } from "@angular/material/toolbar";
import { TeamNewComponent } from "./components/team/new/team.new.component";
import { MatDialogModule } from "@angular/material/dialog";
import { EventListComponent } from "./components/event/list/event.list.component";
import { MatTooltipModule } from "@angular/material/tooltip";
import { UserListComponent } from "./components/user/list/user.list.component";
import { MatSelectModule } from '@angular/material/select';
import { TeamListComponent } from "./components/team/list/team.list.component";
import { EventNewStatusDialogComponent } from './components/event/cards/components/status/event-new-status-dialog.component';
import { RecaptchaModule } from 'ng-recaptcha';
import { NotificationBellComponent } from "./components/notification/bell/notification.bell.component";
import { MatBadgeModule } from "@angular/material/badge";
import { NotificationListComponent } from "./components/notification/list/notification.list.component";
import { MatGridListModule } from "@angular/material/grid-list";
import { NotificationInfoViewComponent } from "./components/notification/templates/info/notification.info.view.component";
import { MatTabsModule } from "@angular/material/tabs";
import { TeamViewComponent } from "./components/team/view/team.view.component";
import { TeamComponent } from "./components/team/team/team.component";
import { MatCardModule } from "@angular/material/card";
import { CustomDialog } from "./components/custom/custom-dialog/custom-dialog.component";
import { ListDetailsComponent } from "./components/custom/list-details/list-details.component";
import { AlertComponent } from "./components/custom/alert/alert.component";
import { ChatTeamComponent } from "./components/chat/team/chat-team.component";
import { ProfileViewComponent } from "./components/profile/view/profile.view.component";
import { ProfileImageComponent } from "./components/profile/image/profile-image.component";
import { ToolbarComponent } from "./components/toolbar/toolbar.component";
import { EventLogComponent } from "./components/eventlog/eventLog.list.component";
import {
  FriendshipOfferButtonComponent
} from "./components/friendship/friendship-offer-button/friendship-offer-button.component";
import { FriendsListComponent } from "./components/friendship/list/friends-list.component";
import { UserTeamComponent } from "./components/team/userTeam/userTeam.component";
import { LineInfoComponent } from './components/line-info/line-info.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { EventCardFactoryComponent } from './components/event/cards/event-card-factory/event-card-factory.component';
import { EventDirective } from "./components/event/event.directive";
import { EventCreateEditCardComponent } from "./components/event/cards/event-create-edit-card/event-create-edit-card.component";
import { EventButtonActionsComponent } from './components/event/cards/components/actions/event-button-actions.component';
import { EventMainViewCardComponent } from "./components/event/cards/event-main-view-card/event-main-view-card.component";
import { EventFinishedViewCardComponent } from './components/event/cards/event-finished-view-card/event-finished-view-card.component';
import { EventStatusComponent } from './components/event/cards/components/event-status/event-status.component';
import {ImageFromStorageComponent} from "./components/custom/image-from-storage/image-from-storage.component";

@NgModule({
  declarations: [

    AppComponent,
    NavMenuComponent,
    LoginComponent,
    RegisterComponent,

    ProfileViewComponent,
    ProfileImageComponent,

    EventListComponent,
    EventNewStatusDialogComponent,
    EventButtonActionsComponent,
    EventStatusComponent,
    ImageFromStorageComponent,
    EventMainViewCardComponent,
    EventCreateEditCardComponent,
    EventFinishedViewCardComponent,
    EventCardFactoryComponent,
    EventDirective,

    TeamViewComponent,
    TeamComponent,
    TeamNewComponent,
    TeamListComponent,
    UserTeamComponent,

    UserListComponent,

    NotFoundComponent,

    ChatTeamComponent,

    EventLogComponent,

    NotificationBellComponent,
    NotificationListComponent,
    NotificationInfoViewComponent,

    FriendshipOfferButtonComponent,
    FriendsListComponent,

    ToolbarComponent,
    DefaultLayoutComponent,
    ListDetailsComponent,
    AlertComponent,
    CustomDialog,
    LineInfoComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,

    MatInputModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatSnackBarModule,
    MatListModule,
    MatToolbarModule,
    ReactiveFormsModule,
    MatMenuModule,
    MatTableModule,
    MatPaginatorModule,
    MatCheckboxModule,
    MatDialogModule,
    MatTooltipModule,
    MatSelectModule,
    RecaptchaModule,
    MatBadgeModule,
    MatGridListModule,
    MatTabsModule,
    MatCardModule,
    InfiniteScrollModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ConnectionRefusedInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    { provide: MatPaginatorIntl, useClass: Pagination}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
