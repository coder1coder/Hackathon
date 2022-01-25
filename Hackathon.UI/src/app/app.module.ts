import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {NavMenuComponent} from "./components/nav-menu/nav-menu.component";
import {LoginComponent} from "./components/login/login.component";
import {RegisterComponent} from "./components/register/register.component";
import {HomeComponent} from "./components/home/home.component";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {MatIconModule} from "@angular/material/icon";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {MatButtonModule} from "@angular/material/button";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {MatListModule} from "@angular/material/list";
import {MatMenuModule} from "@angular/material/menu";
import {NotFoundComponent} from "./components/not-found/not-found.component";
import {MatTableModule} from "@angular/material/table";
import {ProfileComponent} from "./components/profile/profile.component";
import {MatPaginatorIntl, MatPaginatorModule} from "@angular/material/paginator";
import {Pagination} from "./common/Pagination";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {ConnectionRefusedInterceptor} from "./interceptors/connectionRefused.interceptor";
import {AuthInterceptor} from "./interceptors/auth.interceptor";
import {DefaultLayoutComponent} from "./components/layouts/default/default.layout.component";
import {MatToolbarModule} from "@angular/material/toolbar";
import {TeamNewComponent} from "./components/team/new/team.new.component";
import {MatDialogModule} from "@angular/material/dialog";
import {EventListComponent} from "./components/event/list/event.list.component";
import {EventNewComponent} from "./components/event/new/event.new.component";
import {EventViewComponent} from "./components/event/view/event.view.component";
import {MatTooltipModule} from "@angular/material/tooltip";
import {UserListComponent} from "./components/user/list/user.list.component";
import {UserViewComponent} from "./components/user/view/user.view.component";
import {MatSelectModule} from '@angular/material/select';
import {TeamListComponent} from "./components/team/list/team.list.component";
import {TeamViewComponent} from "./components/team/view/team.component";
import {EventNewStatusDialogComponent} from './components/event/status/event-new-status-dialog.component';

@NgModule({
  declarations: [

    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    ProfileComponent,

    EventListComponent,
    EventViewComponent,
    EventNewComponent,
    EventNewStatusDialogComponent,

    TeamViewComponent,
    TeamNewComponent,
    TeamListComponent,

    UserListComponent,
    UserViewComponent,

    NotFoundComponent,

    DefaultLayoutComponent,
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
    MatSelectModule
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
