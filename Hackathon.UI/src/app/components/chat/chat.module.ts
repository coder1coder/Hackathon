import {NgModule} from "@angular/core";
import {ChatTeamComponent} from "./team/chat-team.component";
import {ChatEventComponent} from "./event/chat-event.component";
import {ProfileModule} from "../profile/profile.module";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatListModule} from "@angular/material/list";
import {MatFormFieldModule} from "@angular/material/form-field";
import {ReactiveFormsModule} from "@angular/forms";

@NgModule({
  declarations:[
    ChatTeamComponent,
    ChatEventComponent,
  ],
  imports: [
    ProfileModule,

    MatCheckboxModule,
    MatListModule,
    MatFormFieldModule,
    ReactiveFormsModule
  ],
  exports:[
    ChatTeamComponent,
    ChatEventComponent
  ]
})
export class ChatModule{}
