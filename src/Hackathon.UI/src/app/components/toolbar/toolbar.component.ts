import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { RouterService } from '../../services/router.service';
import { MatDialog } from '@angular/material/dialog';
import {
  CustomDialogComponent,
  ICustomDialogData,
} from '../custom/custom-dialog/custom-dialog.component';
import { FormBuilder } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { ThemeChangeService } from '../../services/theme-change.service';
import { fromMobx } from '../../common/functions/from-mobx.function';
import { IUser } from '../../models/User/IUser';
import { CurrentUserStore } from '../../shared/stores/current-user.store';
import { ProfileUserStore } from '../../shared/stores/profile-user.store';
import { MatToolbar, MatToolbarRow } from '@angular/material/toolbar';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';
import { NotificationBellComponent } from '../notification/bell/notification.bell.component';
import { MatIconButton } from '@angular/material/button';
import { MatMenuTrigger, MatMenu, MatMenuItem } from '@angular/material/menu';
import { NgIf, NgTemplateOutlet } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { ProfileImageComponent } from '../profile/image/profile-image.component';

@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
  imports: [
    MatToolbar,
    MatToolbarRow,
    NavMenuComponent,
    NotificationBellComponent,
    MatIconButton,
    MatMenuTrigger,
    NgIf,
    MatMenu,
    MatMenuItem,
    RouterLink,
    MatIcon,
    NgTemplateOutlet,
    ProfileImageComponent,
  ],
})
export class ToolbarComponent implements OnInit {
  public isDarkMode: boolean = false;
  public userName: string = '';
  public user: IUser | null = null;

  @Input() logoMinWidth: string = 'initial';
  @Input() secondToolbar!: TemplateRef<any> | null;
  @Input() secondToolbarCssClasses!: string;

  private destroy$ = new Subject();

  constructor(
    private routerService: RouterService,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private themeChangeService: ThemeChangeService,
    private currentUserStore: CurrentUserStore,
    private profileUserStore: ProfileUserStore,
  ) {}

  ngOnInit(): void {
    this.currentUserStore.loadCurrentUser();
    this.initSubscribe();
  }

  public logout(): void {
    const data: ICustomDialogData = {
      header: 'Выход',
      content: `Вы уверены, что хотите выйти?`,
      acceptButtonText: `Да`,
    };

    this.dialog
      .open(CustomDialogComponent, { data })
      .afterClosed()
      .subscribe((x) => {
        if (x) {
          this.currentUserStore.clearStore();
          this.profileUserStore.clearStore();
          this.routerService.Profile.Logout();
        }
      });
  }

  private initSubscribe(): void {
    fromMobx(() => this.currentUserStore.currentUser)
      .pipe(takeUntil(this.destroy$))
      .subscribe((curUser) => {
        if (curUser) {
          this.user = curUser;
          this.userName = curUser.fullName ?? (curUser.userName as string);
        }
      });

    fromMobx(() => this.themeChangeService.themeMode)
      .pipe(takeUntil(this.destroy$))
      .subscribe((theme) => {
        if (theme) this.isDarkMode = theme?.isDarkMode;
      });
  }

  toggleApplicationTheme(): void {
    this.themeChangeService.changeMode();
  }
}
