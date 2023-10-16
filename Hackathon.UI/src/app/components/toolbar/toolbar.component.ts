import { Component, Input, OnInit, TemplateRef } from "@angular/core";
import { RouterService } from "../../services/router.service";
import { MatDialog } from "@angular/material/dialog";
import { CustomDialog, ICustomDialogData } from "../custom/custom-dialog/custom-dialog.component";
import { FormBuilder } from "@angular/forms";
import { Subject, takeUntil } from "rxjs";
import { ThemeChangeService } from "../../services/theme-change.service";
import { fromMobx } from "../../common/functions/from-mobx.function";
import { IUser } from "../../models/User/IUser";
import { CurrentUserStore } from "../../shared/stores/current-user.store";

@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})

export class ToolbarComponent implements OnInit {

  public isDarkMode: boolean = false;
  public userName: string;
  public userId: number;

  @Input() logoMinWidth: string = "initial";
  @Input() secondToolbar: TemplateRef<any> | null;
  @Input() secondToolbarCssClasses: string;

  private destroy$ = new Subject();

  constructor(
    private routerService:RouterService,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private themeChangeService: ThemeChangeService,
    private currentUserStore: CurrentUserStore,
  ) {
  }

  ngOnInit() {
    this.currentUserStore.loadCurrentUser();
    this.initSubscribe();
  }

  public get UserId(): number {
    return this.userId;
  }

  public logout(): void {
    const data: ICustomDialogData = {
      header: 'Выход',
      content: `Вы уверены, что хотите выйти?`,
      acceptButtonText: `Да`,
    };

    this.dialog.open(CustomDialog, { data })
      .afterClosed()
      .subscribe(x => {
        if (x) this.routerService.Profile.Logout();
      });
  }

  private initSubscribe(): void {
    fromMobx(() => this.currentUserStore.currentUser)
      .pipe(takeUntil(this.destroy$))
      .subscribe((curUser: IUser) => {
        if (curUser) {
          this.userId = curUser.id;
          this.userName = curUser.fullName ?? curUser.userName;
        }
      })

    fromMobx(() => this.themeChangeService.themeMode)
      .pipe(takeUntil(this.destroy$))
      .subscribe((theme) => {
        this.isDarkMode = theme.isDarkMode;
      })
  }

  toggleApplicationTheme() {
    this.themeChangeService.setMode(!this.themeChangeService.themeMode.isDarkMode);
  }
}
