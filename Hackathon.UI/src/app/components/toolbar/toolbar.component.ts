import {Component, Input, OnInit, TemplateRef} from "@angular/core";
import {AuthService} from "../../services/auth.service";
import {RouterService} from "../../services/router.service";
import {MatDialog} from "@angular/material/dialog";
import {CustomDialog, ICustomDialogData} from "../custom/custom-dialog/custom-dialog.component";
import {MenuItem} from "../../common/MenuItem";
import {FormBuilder, FormGroup} from "@angular/forms";
import {takeUntil} from "rxjs";
import {WithFormComponentBase} from "../WithFormComponentBase";
import {ThemeChangeService} from "../../services/theme-change.service";
import {fromMobx} from "../../common/functions/from-mobx.function";

@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})

export class ToolbarComponent extends WithFormComponentBase implements OnInit {

  public form: FormGroup = this.fb.group({
    toggleControl: [null]
  });

  public userName:string | undefined;
  public userId:number | undefined;

  @Input() logoMinWidth: string = "initial";
  @Input() toolbarMenuItems: MenuItem[] = [];
  @Input() secondToolbar: TemplateRef<any> | null;
  @Input() secondToolbarCssClasses: string;

  constructor(
    private authService:AuthService,
    private routerService:RouterService,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private themeChangeService: ThemeChangeService
  ) {
    super();
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

    this.getFormControl('toggleControl').valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe((darkMode) => this.themeChangeService.setMode(darkMode))

    fromMobx(() => this.themeChangeService.themeMode)
      .pipe(takeUntil(this.destroy$))
      .subscribe((theme) => {
        this.getFormControl('toggleControl').setValue(theme.isDarkMode, {emitEvent: false})
      })
  }
}
