import {NgModule} from "@angular/core";
import {NotificationListComponent} from "./list/notification.list.component";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatIconModule} from "@angular/material/icon";
import {MatMenuModule} from "@angular/material/menu";
import {MatListModule} from "@angular/material/list";
import {MatBadgeModule} from "@angular/material/badge";
import {FriendshipModule} from "../friendship/friendship.module";
import {MatButtonModule} from "@angular/material/button";
import {CommonModule} from "@angular/common";
import {LayoutModule} from "../layouts/layout.module";
import {SharedModule} from "../_shared/shared.module";

@NgModule({
  declarations: [
    NotificationListComponent
  ],
  imports: [
    FriendshipModule,
    LayoutModule,
    SharedModule,

    MatPaginatorModule,
    MatIconModule,
    MatMenuModule,
    MatListModule,
    MatBadgeModule,
    MatButtonModule,
    CommonModule
  ]
})
export class NotificationModule { }
