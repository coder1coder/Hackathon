import {NgModule} from "@angular/core";
import {FriendsListComponent} from "./list/friends-list.component";
import {FriendshipOfferButtonComponent} from "./friendship-offer-button/friendship-offer-button.component";
import {MatButtonModule} from "@angular/material/button";
import {MatIconModule} from "@angular/material/icon";
import {MatListModule} from "@angular/material/list";
import {CommonModule} from "@angular/common";

@NgModule({
  declarations:[
    FriendsListComponent,
    FriendshipOfferButtonComponent
  ],
  imports:[

    MatButtonModule,
    MatIconModule,
    MatListModule,
    CommonModule
  ],
  exports:[
    FriendsListComponent,
    FriendshipOfferButtonComponent
  ]
})
export class FriendshipModule{}
