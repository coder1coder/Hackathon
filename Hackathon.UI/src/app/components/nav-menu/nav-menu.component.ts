import { Component } from '@angular/core';
import { MenuItem } from "../../common/interfaces/menu-item";
import { UserRole } from 'src/app/models/User/UserRole';
import { CurrentUserStore } from "../../shared/stores/current-user.store";
import { fromMobx } from "../../common/functions/from-mobx.function";
import { Observable, Subject, takeUntil } from "rxjs";
import { IUser } from "../../models/User/IUser";
import { AppStateService } from "../../services/app-state.service";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {

  public items: MenuItem[] =[];
  public isLoading$: Observable<boolean> = fromMobx(() => this.appStateService.isLoading);
  private destroy$ = new Subject();

  constructor(
    private currentUserStore: CurrentUserStore,
    private appStateService: AppStateService
  ) {
    this.currentUserStore.loadCurrentUser();
    fromMobx(() => currentUserStore.currentUser)
      .pipe(takeUntil(this.destroy$))
      .subscribe((user: IUser) => {
        if (user) {
          this.items = [
            new MenuItem('/events','События'),
            new MenuItem('/team','Моя команда'),
            new MenuItem('#', 'Администрирование', [UserRole.Administrator],[
              new MenuItem('/users','Пользователи'),
              new MenuItem('/eventLog','Журнал событий'),
              new MenuItem('/approval-applications','Заявки на согласование событий'),
            ])
          ].filter(item=> !item.onlyForRoles || item.onlyForRoles.includes(user.role));
        }
    })
  }
}
