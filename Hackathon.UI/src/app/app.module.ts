import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
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
import {EventsComponent} from "./components/events/events.component";
import {MatListModule} from "@angular/material/list";
import {MatMenuModule} from "@angular/material/menu";
import {NotFoundComponent} from "./components/not-found/not-found.component";
import {TeamsComponent} from "./components/teams/teams.component";
import {MatTableModule} from "@angular/material/table";
import {TeamComponent} from "./components/team/team.component";
import {ProfileComponent} from "./components/profile/profile.component";
import {MatPaginatorIntl, MatPaginatorModule} from "@angular/material/paginator";
import {Pagination} from "./common/Pagination";
import {EventsNewComponent} from "./components/events/new/events.new.component";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {ConnectionRefusedInterceptor} from "./interceptors/connectionRefused.interceptor";
import {AuthInterceptor} from "./interceptors/auth.interceptor";
import {DefaultLayoutComponent} from "./components/layouts/default/default.layout.component";
import {MatToolbarModule} from "@angular/material/toolbar";
import {EventsViewComponent} from "./components/events/view/events.view.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    ProfileComponent,

    EventsComponent,
    EventsViewComponent,
    EventsNewComponent,

    TeamsComponent,
    TeamComponent,

    NotFoundComponent,

    DefaultLayoutComponent
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
    MatCheckboxModule
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
