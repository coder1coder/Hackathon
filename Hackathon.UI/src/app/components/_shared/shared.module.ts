import {NgModule} from "@angular/core";
import {NotificationBellComponent} from "./notification/bell/notification.bell.component";
import {MatMenuModule} from "@angular/material/menu";
import {MatListModule} from "@angular/material/list";
import {MatIconModule} from "@angular/material/icon";
import {ProfileImageComponent} from "./profile/image/profile-image.component";
import {MatBadgeModule} from "@angular/material/badge";
import {NotificationItemComponent} from "./notification/item/notification.item.component";
import {NotificationSystemComponent} from "./notification/item/contents/notification-system.component";
import {
  NotificationFriendshipRejectedComponent
} from "./notification/item/contents/notification-friendship-rejected.component";
import {
  NotificationFriendshipAcceptedComponent
} from "./notification/item/contents/notification-friendship-accepted.component";
import {
  NotificationFriendshipRequestComponent
} from "./notification/item/contents/notification-friendship-request.component";
import {MatButtonModule} from "@angular/material/button";
import {NotificationLayoutComponent} from "./notification/item/layout/notification.layout.component";
import {FriendshipModule} from "../friendship/friendship.module";
import {ViewProfileButtonComponent} from "./profile/view-profile-button/view-profile-button.component";
import {CommonModule} from "@angular/common";

@NgModule({
  declarations: [
    NotificationLayoutComponent,
    NotificationBellComponent,
    NotificationItemComponent,
    ProfileImageComponent,
    NotificationSystemComponent,
    NotificationFriendshipRejectedComponent,
    NotificationFriendshipAcceptedComponent,
    NotificationFriendshipRequestComponent,

    ViewProfileButtonComponent
  ],
  imports: [
    MatMenuModule,
    MatListModule,
    MatIconModule,
    MatBadgeModule,
    MatButtonModule,
    FriendshipModule,
    CommonModule
  ],
  exports: [
    NotificationBellComponent,
    ProfileImageComponent,
    NotificationItemComponent
  ]

})
export class SharedModule { }
