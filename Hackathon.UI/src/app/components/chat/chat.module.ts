import {NgModule} from "@angular/core";
import {ChatTeamComponent} from "./team/chat-team.component";
import {ChatEventComponent} from "./event/chat-event.component";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatListModule} from "@angular/material/list";
import {MatFormFieldModule} from "@angular/material/form-field";
import {ReactiveFormsModule} from "@angular/forms";
import {SharedModule} from "../_shared/shared.module";

@NgModule({
  declarations:[
    ChatTeamComponent,
    ChatEventComponent,
  ],
  imports: [
    MatCheckboxModule,
    MatListModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    SharedModule
  ],
  exports:[
    ChatTeamComponent,
    ChatEventComponent
  ]
})
export class ChatModule{}
