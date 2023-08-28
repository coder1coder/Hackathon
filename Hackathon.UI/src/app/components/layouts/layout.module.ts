import {NgModule} from "@angular/core";
import {DefaultLayoutComponent} from "./default/default.layout.component";
import {ToolbarComponent} from "./toolbar/toolbar.component";
import {MatIconModule} from "@angular/material/icon";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatMenuModule} from "@angular/material/menu";
import {NavMenuComponent} from "./nav-menu/nav-menu.component";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {CommonModule} from "@angular/common";
import {SharedModule} from "../_shared/shared.module";
import {MatButtonModule} from "@angular/material/button";
import {RouterModule} from "@angular/router";

@NgModule({
  declarations:[
    DefaultLayoutComponent,
    ToolbarComponent,
    NavMenuComponent
  ],
  imports: [
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatMenuModule,
    MatProgressSpinnerModule,
    CommonModule,
    SharedModule,
    RouterModule
  ],
  exports: [
    ToolbarComponent,
    DefaultLayoutComponent,
  ]
})
export class LayoutModule{}
