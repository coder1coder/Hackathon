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
import {EventComponent} from "./components/event/event.component";
import {TeamsComponent} from "./components/teams/teams.component";
import {MatTableModule} from "@angular/material/table";
import {TeamComponent} from "./components/team/team.component";
import {ProfileComponent} from "./components/profile/profile.component";
import {AuthInterceptor} from "./services/auth.interceptor";
import {MatPaginatorIntl, MatPaginatorModule} from "@angular/material/paginator";
import {Pagination} from "./common/Pagination";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    ProfileComponent,

    EventsComponent,
    EventComponent,

    TeamsComponent,
    TeamComponent,

    NotFoundComponent
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
    ReactiveFormsModule,
    MatMenuModule,
    MatTableModule,
    MatPaginatorModule
  ],
  providers: [
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
