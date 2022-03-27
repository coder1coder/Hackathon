import {Component} from "@angular/core";
import {TeamModel} from "../../../models/Team/TeamModel";
import {FormControl, FormGroup} from "@angular/forms";
import {Actions} from "../../../common/Actions";
import {ProblemDetails} from "../../../models/ProblemDetails";
import {CreateTeamModel} from "../../../models/Team/CreateTeamModel";
import {MatSnackBar} from "@angular/material/snack-bar";
import {TeamService} from "../../../services/team.service";
import {ActivatedRoute, Route, Router} from "@angular/router";
import {SnackService} from "../../../services/snack.service";

@Component({
  selector: 'team-new',
  templateUrl: 'team.new.component.html',
  styleUrls: ['team.new.component.scss']
})

export class TeamNewComponent
{
  team!: TeamModel;
  eventId!: number;

  isLoading: boolean = false;

  form = new FormGroup({
    name: new FormControl(''),
  })

  constructor(
    private teamService:TeamService,
    private snackBar:SnackService,
    private route: ActivatedRoute
  ) {
    this.eventId = Number(route.snapshot.queryParamMap.get('eventId'));
  }

  submit() {
    let createTeamModel = new CreateTeamModel();
    createTeamModel.name =  this.form.get('name')?.value;

    if (this.eventId > 0)
      createTeamModel.eventId = this.eventId;

    this.teamService.create(createTeamModel)
      .subscribe(_=>{
          this.snackBar.open(`Новая команда добавлена`);
          this.goBack();
        },
        error=>{
          console.log(error)
          let problemDetails: ProblemDetails = <ProblemDetails>error.error;
          this.snackBar.open(problemDetails.detail);
        });
  }

  goBack(){
    history.go(-1);
  }
}

