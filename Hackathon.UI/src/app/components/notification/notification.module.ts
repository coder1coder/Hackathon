import {NgModule} from "@angular/core";
import {NotificationFriendshipAcceptedComponent} from "./item/contents/notification-friendship-accepted.component";
import {NotificationLayoutComponent} from "./item/layout/notification.layout.component";
import {NotificationListComponent} from "./list/notification.list.component";
import {NotificationItemComponent} from "./item/notification.item.component";
import {NotificationSystemComponent} from "./item/contents/notification-system.component";
import {NotificationFriendshipRequestComponent} from "./item/contents/notification-friendship-request.component";
import {NotificationFriendshipRejectedComponent} from "./item/contents/notification-friendship-rejected.component";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatIconModule} from "@angular/material/icon";
import {MatMenuModule} from "@angular/material/menu";
import {MatListModule} from "@angular/material/list";
import {MatBadgeModule} from "@angular/material/badge";
import {ProfileModule} from "../profile/profile.module";
import {FriendshipModule} from "../friendship/friendship.module";
import {MatButtonModule} from "@angular/material/button";
import {CommonModule} from "@angular/common";
import {LayoutModule} from "../layouts/layout.module";
import {NotificationBellComponent} from "../_shared/notification/bell/notification.bell.component";
import {SharedModule} from "../_shared/shared.module";

@NgModule({
  declarations: [
    NotificationListComponent,
    NotificationItemComponent,
    NotificationSystemComponent,
    NotificationFriendshipRequestComponent,
    NotificationLayoutComponent,
    NotificationFriendshipRejectedComponent,
    NotificationFriendshipAcceptedComponent
  ],
  imports: [
    ProfileModule,
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
  ],
  exports:[
  ]

})
export class NotificationModule { }
