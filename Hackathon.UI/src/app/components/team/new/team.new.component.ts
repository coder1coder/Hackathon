import {AfterViewInit, Component, Inject} from "@angular/core";
import {TeamModel} from "../../../models/Team/TeamModel";
import {FormControl, FormGroup} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Actions} from "../../../common/Actions";
import {ProblemDetails} from "../../../models/ProblemDetails";
import {CreateTeamModel} from "../../../models/Team/CreateTeamModel";
import {MatSnackBar} from "@angular/material/snack-bar";
import {TeamsService} from "../../../services/teams.service";

@Component({
  selector: 'team',
  templateUrl: 'team.new.component.html',
  styleUrls: ['team.new.component.scss']
})

export class TeamNewComponent implements AfterViewInit
{
  team!: TeamModel;

  form = new FormGroup({
    name: new FormControl(''),
  })

  constructor(
    private teamService:TeamsService,
    private snackBar:MatSnackBar,
    public dialogRef: MatDialogRef<TeamNewComponent>,
    @Inject(MAT_DIALOG_DATA) private dialogData: any) {
  }

  ngAfterViewInit(): void {
  }

  createTeam() {
    let createTeamModel = new CreateTeamModel();
    createTeamModel.name =  this.form.get('name')?.value;
    createTeamModel.eventId = this.dialogData.eventId;

    this.teamService.create(createTeamModel)
      .subscribe(_=>{
          this.snackBar.open(`Новая команда добавлена`, Actions.OK, { duration: 4 * 1000 });
          this.dialogRef.close(true);
        },
        error=>{
          let problemDetails: ProblemDetails = <ProblemDetails>error.error;
          this.snackBar.open(problemDetails.detail,Actions.OK, { duration: 4 * 1000 });
        });
  }
}

