import {NgModule} from "@angular/core";
import {UserListComponent} from "./list/user.list.component";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatIconModule} from "@angular/material/icon";
import {MatMenuModule} from "@angular/material/menu";
import {ProfileModule} from "../profile/profile.module";
import {LayoutModule} from "../layouts/layout.module";
import {MatTableModule} from "@angular/material/table";

@NgModule({
  declarations:[
    UserListComponent
  ],
  imports:[
    MatPaginatorModule,
    MatIconModule,
    MatMenuModule,
    MatTableModule,

    ProfileModule,
    LayoutModule
  ],
  exports:[
    UserListComponent
  ]
})
export class UserComponentsModule{}
