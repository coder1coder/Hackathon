import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MenuItem } from '../../common/interfaces/menu-item';
import { UserRole } from 'src/app/models/User/UserRole';
import { CurrentUserStore } from '../../shared/stores/current-user.store';
import { fromMobx } from '../../common/functions/from-mobx.function';
import { Observable, Subject, takeUntil } from 'rxjs';
import { IUser } from '../../models/User/IUser';
import { AppStateService } from '../../services/app-state.service';

import { MatMenuItem, MatMenuTrigger, MatMenu } from '@angular/material/menu';
import { RouterLink } from '@angular/router';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'app-nav-menu',
    templateUrl: './nav-menu.component.html',
    styleUrls: ['./nav-menu.component.scss'],
    imports: [MatMenuItem, RouterLink, MatMenuTrigger, MatIcon, MatMenu],
    standalone: true,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NavMenuComponent {
  private currentUserStore = inject(CurrentUserStore);
  private appStateService = inject(AppStateService);

  public items: MenuItem[] = [];
  public isLoading$: Observable<boolean> = fromMobx(() => this.appStateService.isLoading);
  private destroy$ = new Subject();

  constructor() {
    const currentUserStore = this.currentUserStore;

    this.currentUserStore.loadCurrentUser();
    fromMobx(() => currentUserStore.currentUser)
      .pipe(takeUntil(this.destroy$))
      .subscribe((user) => {
        if (user) {
          this.items = [
            new MenuItem('/events', 'События'),
            new MenuItem('/team', 'Моя команда'),
            new MenuItem(
              '#',
              'Администрирование',
              [UserRole.Administrator],
              [
                new MenuItem('/users', 'Пользователи'),
                new MenuItem('/eventLog', 'Журнал событий'),
                new MenuItem('/approval-applications', 'Заявки на согласование событий'),
              ],
            ),
          ].filter((item) => !item.onlyForRoles || item.onlyForRoles.includes(user.role));
        }
      });
  }
}
