import {NgModule} from "@angular/core";
import {DefaultLayoutComponent} from "./default/default.layout.component";
import {ToolbarComponent} from "./toolbar/toolbar.component";
import {MatIconModule} from "@angular/material/icon";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatMenuModule} from "@angular/material/menu";
import {ProfileModule} from "../profile/profile.module";
import {NavMenuComponent} from "./nav-menu/nav-menu.component";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";

@NgModule({
  declarations:[
    DefaultLayoutComponent,
    ToolbarComponent,
    NavMenuComponent
  ],
  imports: [
    ProfileModule,

    MatIconModule,
    MatToolbarModule,
    MatMenuModule,
    MatProgressSpinnerModule
  ],
  exports:[
    DefaultLayoutComponent,
    ToolbarComponent,
  ]
})
export class LayoutModule{}
