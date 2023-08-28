import {NgModule} from "@angular/core";
import {UserListComponent} from "./list/user.list.component";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatIconModule} from "@angular/material/icon";
import {MatMenuModule} from "@angular/material/menu";
import {MatTableModule} from "@angular/material/table";
import {SharedModule} from "../_shared/shared.module";
import {FriendshipModule} from "../friendship/friendship.module";
import {CommonComponentsModule} from "../custom/common-components.module";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {MatTabsModule} from "@angular/material/tabs";
import {CommonModule} from "@angular/common";
import {ReactiveFormsModule} from "@angular/forms";
import {MatButtonModule} from "@angular/material/button";
import {ProfileViewComponent} from "./view/profile.view.component";

@NgModule({
  declarations:[
    UserListComponent,
    ProfileViewComponent
  ],
  imports: [
    MatPaginatorModule,
    MatIconModule,
    MatMenuModule,
    MatTableModule,

    FriendshipModule,
    CommonComponentsModule,

    MatFormFieldModule,
    MatInputModule,
    MatTabsModule,
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    SharedModule,

  ],
  exports: [
    UserListComponent
  ]
})
export class UserComponentsModule{}
