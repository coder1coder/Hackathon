import {Component} from "@angular/core";
import {Team} from "../../../models/Team/Team";
import {FormControl, FormGroup} from "@angular/forms";
import {IProblemDetails} from "../../../models/IProblemDetails";
import {CreateTeamModel} from "../../../models/Team/CreateTeamModel";
import {TeamClient} from "../../../services/team-client.service";
import {ActivatedRoute} from "@angular/router";
import {SnackService} from "../../../services/snack.service";
import { TeamType } from "src/app/models/Team/TeamType.";

@Component({
  selector: 'team-new',
  templateUrl: 'team.new.component.html',
  styleUrls: ['team.new.component.scss']
})

export class TeamNewComponent
{
  team!: Team;
  eventId!: number;

  isLoading: boolean = false;

  selectedTeamType : number = 0;

  teamTypes: TeamType[] = [
    {id: 0, name: 'Закрытый'},
    {id: 1, name: 'Открытый'},
  ];

  form = new FormGroup({
    name: new FormControl(''),
  })

  constructor(
    private teamService:TeamClient,
    private snackBar:SnackService,
    private route: ActivatedRoute
  ) {
    this.eventId = Number(route.snapshot.queryParamMap.get('eventId'));
  }

  submit() {
    let createTeamModel = new CreateTeamModel();
    createTeamModel.name =  this.form.get('name')?.value;
    createTeamModel.type = this.selectedTeamType;

    if (this.eventId > 0)
      createTeamModel.eventId = this.eventId;

    this.teamService.create(createTeamModel)
      .subscribe(_=>{
          this.snackBar.open(`Новая команда добавлена`);
          this.goBack();
        },
        error=>{
          let problemDetails: IProblemDetails = <IProblemDetails>error.error;
          this.snackBar.open(problemDetails.detail);
        });
  }

  goBack(){
    history.go(-1);
  }
}

