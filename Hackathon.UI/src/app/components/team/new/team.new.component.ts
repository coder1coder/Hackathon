import { Component, OnDestroy } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { CreateTeamModel } from "../../../models/Team/CreateTeamModel";
import { TeamClient } from "../../../services/team-client.service";
import { ActivatedRoute } from "@angular/router";
import { SnackService } from "../../../services/snack.service";
import { TeamType } from "src/app/models/Team/TeamType.";
import { takeUntil } from "rxjs";
import { ErrorProcessorService } from "../../../services/error-processor.service";
import { WithFormBaseComponent } from "../../../common/base-components/with-form-base.component";

@Component({
  selector: 'team-new',
  templateUrl: 'team.new.component.html',
  styleUrls: ['team.new.component.scss']
})

export class TeamNewComponent extends WithFormBaseComponent implements OnDestroy {

  public selectedTeamType: number = 0;
  public teamTypes: TeamType[] = [
    {id: 0, name: 'Закрытый'},
    {id: 1, name: 'Открытый'},
  ];

  public form = this.fb.group({
    name: [null]
  });

  private readonly eventId: number;

  constructor(
    private teamService:TeamClient,
    private snackBar:SnackService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private errorProcessor: ErrorProcessorService,
  ) {
    super();
    this.eventId = Number(route.snapshot.queryParamMap.get('eventId'));
  }

  public submit(): void {
    const createTeamModel = new CreateTeamModel();
    createTeamModel.name =  this.getFormControl('name')?.value;
    createTeamModel.type = this.selectedTeamType;
    if (this.eventId > 0)
      createTeamModel.eventId = this.eventId ?? null;

    this.teamService.create(createTeamModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.snackBar.open(`Новая команда добавлена`);
          this.goBack();
        },
        error: (error) => this.errorProcessor.Process(error),
      });
  }

  public goBack(): void {
    history.go(-1);
  }
}

