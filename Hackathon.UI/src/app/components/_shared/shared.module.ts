import {NgModule} from "@angular/core";
import {NotificationBellComponent} from "./notification/bell/notification.bell.component";
import {MatMenuModule} from "@angular/material/menu";
import {MatListModule} from "@angular/material/list";
import {MatIconModule} from "@angular/material/icon";

@NgModule({
  declarations: [
    NotificationBellComponent
  ],
  imports: [
    MatMenuModule,
    MatListModule,
    MatIconModule
  ],
  exports:[
    NotificationBellComponent
  ]

})
export class SharedModule { }
