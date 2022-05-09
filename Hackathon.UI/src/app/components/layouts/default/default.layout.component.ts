import {AfterViewInit, Component, Input} from "@angular/core";
import {AuthService} from "../../../services/auth.service";
import {CustomDialog, ICustomDialogData} from "../../custom/custom-dialog/custom-dialog.component";
import {RouterService} from "../../../services/router.service";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'layout-default',
  templateUrl: './default.layout.component.html',
  styleUrls: ['./default.layout.component.scss'],
})

export class DefaultLayoutComponent implements AfterViewInit {

  @Input() title!: string;
  @Input() isLoading: boolean = false;

  UserName:string | undefined;

  constructor(
    private authService:AuthService,
    private routerService:RouterService,
    private dialog: MatDialog
    ) {}

  ngAfterViewInit(): void {
    this.authService.getCurrentUser()?.subscribe(x=>{
      if (x != null)
        this.UserName = x.fullName ?? x.userName
    })
  }

  public logout(){
    let data: ICustomDialogData = {
      header: 'Выход',
      content: `Вы уверены, что хотите выйти?`,
      acceptButtonText: `Да`
    };

    this.dialog.open(CustomDialog, { data })
      .afterClosed()
      .subscribe(x =>
      {
        if (x) this.routerService.Profile.Logout();
      });
  }
}

