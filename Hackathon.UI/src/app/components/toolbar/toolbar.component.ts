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

@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent implements OnInit {
  public isDarkMode: boolean = false;
  public userName: string;
  public user: IUser;

  @Input() logoMinWidth: string = 'initial';
  @Input() secondToolbar: TemplateRef<any> | null;
  @Input() secondToolbarCssClasses: string;

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
      .subscribe((curUser: IUser) => {
        if (curUser) {
          this.user = curUser;
          this.userName = curUser.fullName ?? curUser.userName;
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
