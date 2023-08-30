import {Component, Input, OnInit, TemplateRef} from "@angular/core";
import {AuthService} from "../../services/auth.service";
import {RouterService} from "../../services/router.service";
import {MatDialog} from "@angular/material/dialog";
import {CustomDialog, ICustomDialogData} from "../custom/custom-dialog/custom-dialog.component";
import {MenuItem} from "../../common/MenuItem";
import {FormBuilder} from "@angular/forms";
import {Subject, takeUntil} from "rxjs";
import {ThemeChangeService} from "../../services/theme-change.service";
import {fromMobx} from "../../common/functions/from-mobx.function";

@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})

export class ToolbarComponent implements OnInit {

  public isDarkMode: boolean = false;

  public userName:string | undefined;
  public userId:number | undefined;

  @Input() logoMinWidth: string = "initial";
  @Input() secondToolbar: TemplateRef<any> | null;
  @Input() secondToolbarCssClasses: string;

  private destroy$ = new Subject();

  constructor(
    private authService:AuthService,
    private routerService:RouterService,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private themeChangeService: ThemeChangeService
  ) {
  }

  ngOnInit() {
   this.initSubscribe();
  }

  public logout(): void {
    const data: ICustomDialogData = {
      header: 'Выход',
      content: `Вы уверены, что хотите выйти?`,
      acceptButtonText: `Да`
    };

    this.dialog.open(CustomDialog, { data })
      .afterClosed()
      .subscribe(x => {
        if (x) this.routerService.Profile.Logout();
      });
  }

  private initSubscribe(): void {
    this.authService.getCurrentUser()
      ?.subscribe((curUser) => {
        if (curUser != null) {
          this.userId = curUser.id;
          this.userName = curUser.fullName ?? curUser.userName
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
