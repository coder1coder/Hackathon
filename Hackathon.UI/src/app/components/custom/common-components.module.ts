import {NgModule} from "@angular/core";
import {AlertComponent} from "./alert/alert.component";
import {MatIconModule} from "@angular/material/icon";

@NgModule({
  declarations:[
    AlertComponent
  ],
  imports:[
    MatIconModule
  ],
  exports:[
    AlertComponent
  ]
})
export class CommonComponentsModule{}
