import { Component, OnDestroy, inject } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CreateTeamModel } from '../../../models/Team/CreateTeamModel';
import { ActivatedRoute } from '@angular/router';
import { SnackService } from '../../../services/snack.service';
import { TeamType } from 'src/app/models/Team/TeamType.';
import { takeUntil } from 'rxjs';
import { ErrorProcessorService } from '../../../services/error-processor.service';
import { WithFormBaseComponent } from '../../../common/base-components/with-form-base.component';
import { TeamsClient } from 'src/app/clients/teams.client';
import { DefaultLayoutComponent } from '../../layouts/default/default.layout.component';
import { MatFormField, MatLabel, MatInput } from '@angular/material/input';
import { MatSelect, MatOption } from '@angular/material/select';

import { MatButton } from '@angular/material/button';

@Component({
    selector: 'team-new',
    templateUrl: 'team.new.component.html',
    styleUrls: ['team.new.component.scss'],
    imports: [DefaultLayoutComponent, FormsModule, ReactiveFormsModule, MatFormField, MatLabel, MatInput, MatSelect, MatOption, MatButton]
})
export class TeamNewComponent extends WithFormBaseComponent implements OnDestroy {
  private teamsClient = inject(TeamsClient);
  private snackBar = inject(SnackService);
  private route = inject(ActivatedRoute);
  private fb = inject(FormBuilder);
  private errorProcessor = inject(ErrorProcessorService);

  public selectedTeamType: number = 0;
  public teamTypes: TeamType[] = [
    { id: 0, name: 'Закрытый' },
    { id: 1, name: 'Открытый' },
  ];

  public form = this.fb.group({
    name: [null],
  });

  private readonly eventId: number;

  constructor() {
    super();
    const route = this.route;

    this.eventId = Number(route.snapshot.queryParamMap.get('eventId'));
  }

  public submit(): void {
    const createTeamModel: CreateTeamModel = new CreateTeamModel();
    createTeamModel.name = this.getFormControl('name')?.value;
    createTeamModel.type = this.selectedTeamType;
    if (this.eventId > 0) createTeamModel.eventId = this.eventId ?? null;

    this.teamsClient
      .create(createTeamModel)
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
