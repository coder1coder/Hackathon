import {ProfileImageComponent} from "./image/profile-image.component";
import {ProfileViewComponent} from "./view/profile.view.component";
import {ViewProfileButtonComponent} from "./view-profile-button/view-profile-button.component";
import {NgModule} from "@angular/core";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {FriendshipModule} from "../friendship/friendship.module";
import {MatTabsModule} from "@angular/material/tabs";
import {MatIconModule} from "@angular/material/icon";
import {CommonModule} from "@angular/common";
import {CommonComponentsModule} from "../custom/common-components.module";
import {ReactiveFormsModule} from "@angular/forms";
import {MatButtonModule} from "@angular/material/button";
import {LayoutModule} from "../layouts/layout.module";

@NgModule({
  declarations: [
    ProfileImageComponent,
    ProfileViewComponent,
    ViewProfileButtonComponent
  ],
  exports: [
    ProfileImageComponent,
    ViewProfileButtonComponent
  ],
  imports: [
    FriendshipModule,
    CommonComponentsModule,
    LayoutModule,

    MatFormFieldModule,
    MatInputModule,
    MatTabsModule,
    MatIconModule,
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule
  ]
})

export class ProfileModule {}
